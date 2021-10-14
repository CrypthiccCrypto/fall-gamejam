using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float BULLET_SPEED = 20.0f;
    [SerializeField] private float DEATH_TIME = 2f;
    private float timer = 0.0f;
    private Vector3 bullet_dir;
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
