using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private float aspect_ratio;
    [SerializeField] private Rigidbody2D player;
    private Vector3 offset;
    private int height;
    private bool isInside = true;

    void Start() {
        float tmp = aspect_ratio * width;
        height = (int)tmp;

        transform.localScale = new Vector3(width, height, 1);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            isInside = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            isInside = false;
        }
    }

    void Update() {
        moveCam();
    }

    void moveCam() {
        if(!isInside) {
            Vector2 tmp = (Vector2) transform.position;
            tmp += player.velocity * Time.deltaTime;
            transform.position = new Vector3(tmp.x, tmp.y, -1);
        }
    }
}
