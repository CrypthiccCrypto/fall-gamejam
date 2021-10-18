using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private CameraShaker camShake;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float MAX_HEALTH;
    [SerializeField] private float KNOCKBACK_FRIC_COEFF;
    [SerializeField] private float FRIC_COEFF;
    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float THRUST_CONST;
    [SerializeField] private float KNOCK_CONST;
    [SerializeField] public float damage;
    [SerializeField] private int POINTS;
    private AudioManager audioManager;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 knockbackVelocity;
    private Vector2 thrust;
    private float health;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Boma") {
            Boma bomaScript = other.gameObject.GetComponent<Boma>();
            this.health -= bomaScript.damage/Mathf.Pow(bomaScript.curr_Radius, 1.2f);
            if (this.health < 0) { Boma.can_boma += POINTS; }
        }
    }
    void Start() {
        this.health = MAX_HEALTH;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if(GameManager.isGameOver) { this.health = -1;  checkDeath(); }
        else { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); }
    }

    void Update() {
        checkPlayerDeath();
        checkDeath();
        aim();
        move();
        setColor();
    }   

    void checkPlayerDeath() {
        if(GameManager.isGameOver)
            this.health = -1;
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

            if(!GameManager.isGameOver) { camShake.shakeCam(); Score.SCORE += POINTS; audioManager.playAudio(0, 0.69f);}
        }
    }
}
