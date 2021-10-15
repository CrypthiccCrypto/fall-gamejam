using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraShaker camShake;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float MAX_HEALTH;
    [SerializeField] private float KNOCKBACK_FRIC_COEFF;
    [SerializeField] private float FRIC_COEFF;
    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float THRUST_CONST;
    [SerializeField] private float KNOCK_CONST;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 knockbackVelocity;
    private Vector2 thrust;
    private float health;

    void Start() {
        this.health = MAX_HEALTH;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
    }

    void Update() {
        aim();
        move();
        setColor();
        checkDeath();
    }

    void aim() {
        Vector2 direction = (player.transform.position - this.transform.position).normalized;
        thrust = direction * THRUST_CONST;
    }

    void move() {
        rb.velocity += knockbackVelocity * Time.deltaTime;
        rb.velocity += thrust * Time.deltaTime;
        
        Mathf.Clamp(rb.velocity.magnitude, 0, MAX_SPEED);
        knockbackVelocity *= KNOCKBACK_FRIC_COEFF;
        rb.velocity *= FRIC_COEFF;
    }

    void setColor() {
        sprite.color = new Color(1.0f - Mathf.Pow((MAX_HEALTH - health)/MAX_HEALTH, 2), 0, 0, 1.0f - knockbackVelocity.magnitude/KNOCK_CONST);
    }
    public void hit(Vector3 dir, float damage) {
        knockbackVelocity = dir * KNOCK_CONST;
        health -= damage;
    }

    void checkDeath() {
        if(this.health < 0) {
            Destroy(gameObject);
            Instantiate(effectPrefab, transform.position, transform.rotation);
            camShake.shakeCam();
        }
    }
}
