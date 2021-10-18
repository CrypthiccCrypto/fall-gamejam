using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject basicEnemyPrefab;
    [SerializeField] GameObject splitEnemyPrefab;
    [SerializeField] Transform goPro;
    [SerializeField] private float CAMERA_THICKNESS;
    [SerializeField] private float CAMERA_TALLNESS;
    [SerializeField] private float FREQUENCY;
    [SerializeField] private float START_FREQUENCY;
    [SerializeField] private float END_FREQUENCY;
    private float timer = 0.0f;

    void Start() {
        FREQUENCY = START_FREQUENCY;
    }
    void Update() {
        timer += Time.deltaTime;
        if(timer > FREQUENCY) {
            Dischurj();
            timer = 0;
        }
        
        if(FREQUENCY != END_FREQUENCY)
            updateFrequency();
    }
    void updateFrequency() {
        FREQUENCY -= Time.deltaTime/300;
        FREQUENCY = Mathf.Clamp(FREQUENCY, END_FREQUENCY, START_FREQUENCY);
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

        float prob = Random.Range(0.0f, 1.0f);
        if(prob < 0.25f)
            Instantiate(splitEnemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
        else
            Instantiate(basicEnemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
