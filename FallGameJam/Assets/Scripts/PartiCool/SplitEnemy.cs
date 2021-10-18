using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : MonoBehaviour
{
    private Transform player;
    private CameraShaker camShake;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject splitEnemy;
    [SerializeField] private float MAX_HEALTH;
    [SerializeField] private float KNOCKBACK_FRIC_COEFF;
    [SerializeField] private float FRIC_COEFF;
    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float THRUST_CONST;
    [SerializeField] private float KNOCK_CONST;
    [SerializeField] public float damage;
    [SerializeField] private int POINTS;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 knockbackVelocity;
    private AudioManager audioManager;
    private Vector2 thrust;
    private float health;
    private int stage = 1;

    void Start() {
        this.damage /= this.stage;
        this.health = MAX_HEALTH/stage;
        this.POINTS = (int)(this.POINTS/stage);

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        
        transform.localScale = new Vector3(1.0f/stage, 1.0f/stage, 1);

        if(GameManager.isGameOver) { this.health = -1; checkDeath(); }
        else { player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Boma") {
            Boma bomaScript = other.gameObject.GetComponent<Boma>();
            this.health -= bomaScript.damage/Mathf.Pow(bomaScript.curr_Radius, 1.2f);
            if (this.health < 0) { Boma.can_boma += POINTS; }
        }
    }

    void Update() {
        checkPlayerDeath();
        checkDeath();
        setColor();
    }

    void FixedUpdate() {
        aim();
        move();
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
        rb.velocity += knockbackVelocity * Time.fixedDeltaTime;
        rb.velocity += thrust * Time.fixedDeltaTime;
        
        Mathf.Clamp(rb.velocity.magnitude, 0, MAX_SPEED);
        knockbackVelocity *= KNOCKBACK_FRIC_COEFF;
        rb.velocity *= FRIC_COEFF;
    }

    void setColor() {
        sprite.color = new Color(0, 0, 1.0f - Mathf.Pow((MAX_HEALTH - health)/MAX_HEALTH, 2), 1.0f - knockbackVelocity.magnitude/KNOCK_CONST);
    }
    public void hit(Vector3 dir, float damage) {
        knockbackVelocity = dir * KNOCK_CONST;
        health -= damage;
    }
    void checkDeath() {
        if(this.health < 0) {
            Destroy(gameObject);
            
            ParticleSystem p_tmp = Instantiate(effectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
            p_tmp.emission.SetBurst(0, new ParticleSystem.Burst(0f, (int)(MAX_HEALTH/(stage*stage)), 1, 0.01f));
            p_tmp.Play();

            if(stage < 3) {
                Vector3 pos1 = Random.insideUnitCircle*1.5f;
                Vector3 pos2 = Random.insideUnitCircle*1.5f;

                GameObject tmp = Instantiate(splitEnemy, transform.position + pos1, Quaternion.identity);
                tmp.GetComponent<SplitEnemy>().setStage(stage + 1);
                tmp.GetComponent<SplitEnemy>().enabled = true;
                tmp.GetComponent<CircleCollider2D>().enabled = true;

                GameObject tmp2 = Instantiate(splitEnemy, transform.position + pos2, Quaternion.identity);
                tmp2.GetComponent<SplitEnemy>().setStage(stage + 1);
                tmp2.GetComponent<SplitEnemy>().enabled = true;
                tmp2.GetComponent<CircleCollider2D>().enabled = true;
            }

            if(!GameManager.isGameOver) { camShake.shakeCam(); Score.SCORE += POINTS; audioManager.playAudio(0, 0.69f/stage);}
        }
    }
    void setStage(int stage) {
        this.stage = stage;
    }
}
