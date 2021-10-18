using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private float BULLET_COOLDOWN = 1f;
    [SerializeField] private Transform crosshair;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shobHead;
    [SerializeField] private float SHOB_HEAD_RADIUS = 0.8f;
    [SerializeField] private AudioManager bulletAudioManager;
    //[SerializeField] private GameManager gameManager;
    private SpriteRenderer sprite;
    private float bullet_timer;
    private bool isShooting = false;
    private Vector3 facing; //vector pointing from player to crosshair
    private float angle;

    

    void Start() 
    {   
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update() 
    {
        playerShoot();
        playerShobHead();
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
    void spawnBullet() {
        bulletAudioManager.playAudio(0, 0.15f);
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
}
