using System.Collections;
using UnityEngine;

public class Boma : MonoBehaviour // a.k.a. Dodoma
{
    [SerializeField] private float INITIAL_BOMA_RADIUS;
    [SerializeField] private float MAX_PULSATE_RADIUS;
    [SerializeField] private float FINAL_BOMA_RADIUS;
    [SerializeField] private float RATE;
    [SerializeField] AudioManager audioManager;
    public float damage = 400;
    public float curr_Radius = 0;
    static public int can_boma = 100;
    public CircleCollider2D cd;
    private float ticks = 0.0f;

    private bool powered_up = false;

    void Start() {
        can_boma = 100;
        curr_Radius = INITIAL_BOMA_RADIUS;
        cd = GetComponent<CircleCollider2D>();
        cd.enabled = false;
    }
    IEnumerator expand() {
        Time.timeScale = 0.5f;
        curr_Radius = INITIAL_BOMA_RADIUS;
        cd.enabled = true;

        while(curr_Radius < FINAL_BOMA_RADIUS) {
            curr_Radius += RATE * Time.deltaTime;
            if (curr_Radius >= FINAL_BOMA_RADIUS/2) { Time.timeScale = 1.0f; }
            yield return null;
        }

        curr_Radius = INITIAL_BOMA_RADIUS;
        cd.enabled = false;
        powered_up = false;
    }
    void Update() {
        if(can_boma < Score.SCORE) {
            pulsating();
            if (!powered_up) { powered_up = true; audioManager.playAudio(1, 1); }
        }
        setSize();
    }
    void setSize() {
        this.transform.localScale = new Vector3(curr_Radius, curr_Radius, 1);
    }
    void pulsating() {
        ticks += Time.deltaTime;
        if(ticks > 2 * Mathf.PI) { ticks -= 2 * Mathf.PI; }
        curr_Radius = (INITIAL_BOMA_RADIUS + MAX_PULSATE_RADIUS)/2.0f + (MAX_PULSATE_RADIUS - INITIAL_BOMA_RADIUS)/2.0f * Mathf.Sin(5 * ticks);
    }
}
