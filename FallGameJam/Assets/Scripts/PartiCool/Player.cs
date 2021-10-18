using UnityEngine;
using System.Collections;

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
    [SerializeField] public Transform shobHead;
    [SerializeField] public float SHOB_HEAD_RADIUS = 0.8f;
    [SerializeField] private float KNOCKBACK;
    [SerializeField] private GameObject sparkXPrefab;
    [SerializeField] private GameObject sparkYPrefab;
    [SerializeField] private float MAX_HEALTH;
    [SerializeField] private SpriteRenderer healthObject;
    [SerializeField] private Transform healthMask;
    [SerializeField] private Boma boma;
    [SerializeField] private GameObject damageParticleEffectPrefab;
    [SerializeField] private AudioManager bulletAudioManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GunReload gunReload;

    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private SpriteRenderer sprite;
    private float bullet_timer;
    private bool isShooting = false;
    private Vector3 facing; //vector pointing from player to crosshair
    private float angle;
    private float health;
    private bool isInvinicble = false;
    

    void Start() 
    {   
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        gunReload = GetComponent<GunReload>();

        health = MAX_HEALTH;
    }
    void Update() 
    {
        playerShoot();
        playerShobHead();
        kawaiiEye();
        checkDeath();
    }

    void FixedUpdate() {
        playerInput();
        limit_to_field();
    }

    void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy" && !isInvinicble) {
            isInvinicble = true;

            Vector2 direction = (this.transform.position - other.transform.position).normalized;
            float damage_taken = other.gameObject.GetComponent<Enemy>().damage;
            direction *= KNOCKBACK * damage_taken;
            rb.velocity += direction * Time.fixedDeltaTime;
            health -= damage_taken;

            StartCoroutine(invincibility());
        }
        else if(other.gameObject.tag == "SplitEnemy" && !isInvinicble) {
            isInvinicble = true;
            
            Vector2 direction = (this.transform.position - other.transform.position).normalized;
            float damage_taken = other.gameObject.GetComponent<SplitEnemy>().damage;
            direction *= KNOCKBACK *damage_taken;
            rb.velocity += direction * Time.fixedDeltaTime;
            health -= damage_taken;
            
            StartCoroutine(invincibility());
        }
    }

    IEnumerator invincibility() {
        Instantiate(damageParticleEffectPrefab, this.transform.position, Quaternion.identity);
        audioManager.playAudio(2, 0.2f);
        updateHealth();
        
        for(int i = 1; i <= 90; i++) {
            sprite.color = new Color(24f/255, 168f/255, 31f/255, Mathf.Sin(17*Mathf.PI*(90-i)*(90-i)*(90-i)/(180*90*90) + Mathf.PI/2)*0.4f + 0.6f);

            yield return new WaitForSeconds(1.0f/60);
        }

        isInvinicble = false;
    }
    
    void updateHealth() {
        gameManager.updateHealth(health, MAX_HEALTH);
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
        if(Input.GetKeyDown("space") && Boma.can_boma < Score.SCORE) {
            Boma.can_boma = Score.SCORE + 100;
            boma.StartCoroutine("expand");
        }
        if(Input.GetKeyDown(KeyCode.R) && !gunReload.isReloading && gunReload.curr_Mag != gunReload.MAX_MAG_SIZE) {
            gunReload.isReloading = true;
            audioManager.playAudio(4, 0.3f);
            gunReload.reloadAnimation();
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
        if(gunReload.curr_Mag > 0 && !gunReload.isReloading) {
            gunReload.curr_Mag--;
            bulletAudioManager.playAudio(0, 0.15f);
            Instantiate(bulletPrefab, transform.position + 1.55f * (shobHead.transform.position - transform.position), Quaternion.Euler(0, 0, angle*Mathf.Rad2Deg)).GetComponent<Bullet>().assignDirection(facing);
        }
        else if (!gunReload.isReloading){
            gunReload.isReloading = true;
            audioManager.playAudio(4, 0.3f);
            gunReload.reloadAnimation();
        }
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
            Vector2 central_force = new Vector2(0, 0);
            
            if (transform.position.x < -WIDTH) { Instantiate((sparkYPrefab), this.transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity); central_force.x += 1; }
            else if (transform.position.x > +WIDTH) { Instantiate((sparkYPrefab), this.transform.position + new Vector3(+0.5f, 0, 0), Quaternion.identity); central_force.x -= 1; }
            
            if (transform.position.y < -HEIGHT) { Instantiate((sparkXPrefab), this.transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity); central_force.y += 1;  }
            else if (transform.position.y > +HEIGHT) { Instantiate((sparkXPrefab), this.transform.position + new Vector3(0, +0.5f, 0), Quaternion.identity); central_force.y -= 1;  }
            
            central_force = central_force.normalized;
            central_force *= FORCE_FIELD;
            rb.velocity += central_force * Time.fixedDeltaTime;
        }
    }

    void checkDeath() {
        if (this.health < 0) {
            Time.timeScale = 0.5f;
            Instantiate(damageParticleEffectPrefab, this.transform.position, Quaternion.identity);
            audioManager.playAudio(3, 0.2f);
            GameManager.isGameOver = true;
            gameManager.updateHealth(health, MAX_HEALTH);
            gameManager.setGameOver();
            
            gameObject.SetActive(false);
        }
    }
}
