using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject basicEnemyPrefab;
    [SerializeField] Transform goPro;
    [SerializeField] private float CAMERA_THICKNESS;
    [SerializeField] private float CAMERA_TALLNESS;
    [SerializeField] private float FREQUENCY;
    private float timer = 0.0f;

    void Update() {
        timer += Time.deltaTime;
        if(timer > FREQUENCY) {
            Dischurj();
            timer = 0;
        }
    }
    void Dischurj() {
        float x = goPro.position.x;
        float y = goPro.position.y;

        if (Random.Range(0.0f, 1.0f) < 0.5f) {
            x += (Random.Range(0.0f, 1.0f) < 0.5f ? -1f : +1f) * CAMERA_THICKNESS;
            y += Random.Range(-1.0f, 1.0f) * CAMERA_TALLNESS;
        } 
        else {
            y += (Random.Range(0.0f, 1.0f) < 0.5f ? -1f : +1f) * CAMERA_TALLNESS;
            x += Random.Range(-1.0f, 1.0f) * CAMERA_THICKNESS;
        }

        Instantiate(basicEnemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
