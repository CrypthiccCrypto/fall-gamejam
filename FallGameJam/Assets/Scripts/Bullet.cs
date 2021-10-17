using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BULLET_SPEED = 20.0f;
    [SerializeField] private float DEATH_TIME = 2f;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float damage;
    private float timer = 0.0f;
    private Vector3 bullet_dir;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            Instantiate(effectPrefab, transform.position, transform.rotation);
            other.gameObject.GetComponent<Enemy>().hit(bullet_dir, damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "SplitEnemy") {
            Instantiate(effectPrefab, transform.position, transform.rotation);
            other.gameObject.GetComponent<SplitEnemy>().hit(bullet_dir, damage);
            Destroy(gameObject);
        }
    }
    void Update() {
        transform.position += bullet_dir * BULLET_SPEED * Time.deltaTime;

        timer += Time.deltaTime;
        if(timer > DEATH_TIME)
            Destroy(gameObject);
    }
    public void assignDirection(Vector3 bullet_dir) {
        this.bullet_dir = bullet_dir;
    }
}
