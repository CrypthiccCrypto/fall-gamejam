using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float HEIGHT;
    [SerializeField] private float WIDTH;
    [SerializeField] private float FORCE_FIELD;
    [SerializeField] private float SPEED = 1.5f;
    [SerializeField] private float FRIC_COEFF = 0.92f;
    [SerializeField] private float BULLET_COOLDOWN = 1f;
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject k_Eye;
    [SerializeField] private Transform crosshair;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shobHead;
    [SerializeField] private float SHOB_HEAD_RADIUS = 0.8f;
    private Rigidbody2D rb;
    private float bullet_timer;
    private bool isShooting = false;
    private Vector3 facing; //vector pointing from player to crosshair
    private float angle;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update() 
    {
        playerShoot();
        playerShobHead();
        kawaiiEye();
    }

    void FixedUpdate() {
        playerInput();
        limit_to_field();
    }

    void playerInput() 
    {
        float x_inp = Input.GetAxisRaw("Horizontal");
        float y_inp = Input.GetAxisRaw("Vertical");

        Vector2 mov_dir = (new Vector3(x_inp, y_inp)).normalized;
        rb.velocity *= FRIC_COEFF;
        rb.velocity += mov_dir * SPEED;
    }

    void playerShoot() {
        if(Input.GetMouseButtonDown(0)) {
            isShooting = true;
            spawnBullet();
        }
        if(isShooting) {
            bullet_timer += Time.deltaTime;
        }
        if(Input.GetMouseButton(0) && bullet_timer > BULLET_COOLDOWN) {
            spawnBullet();
            bullet_timer = 0f;
        }
        if(Input.GetMouseButtonUp(0)) {
            isShooting = false;
            bullet_timer = 0f;
        }
    }
    void kawaiiEye() {
        if(rb.velocity.magnitude < 0.2) {
            eye.SetActive(true);
            k_Eye.SetActive(false);
        }
        else {
            eye.SetActive(false);
            k_Eye.SetActive(true);
        }
    }

    void spawnBullet() {
        Instantiate(bulletPrefab, transform.position + 1.55f * (shobHead.transform.position - transform.position), Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg)).GetComponent<Bullet>().assignDirection(facing);
    }
    void playerShobHead() {
        Vector3 dir = (crosshair.position - transform.position);
        dir.z = 0;
        facing = dir.normalized;
        angle = Mathf.Atan2(facing.y, facing.x);

        shobHead.transform.position = new Vector3(SHOB_HEAD_RADIUS*Mathf.Cos(angle), SHOB_HEAD_RADIUS*Mathf.Sin(angle), 0) + transform.position;
        shobHead.rotation = Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg);
    }

    void limit_to_field() {
        if (Mathf.Abs(this.transform.position.x) > WIDTH || Mathf.Abs(this.transform.position.y) > HEIGHT) {
            Vector2 central_force = - this.transform.position.normalized;
            central_force *= FORCE_FIELD;
            rb.velocity += central_force * Time.fixedDeltaTime;
        }
    }
}
