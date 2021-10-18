using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private SpriteRenderer healthObject;
    [SerializeField] private Transform healthMask;
    [SerializeField] private Player player;

    void Start() {
        Time.timeScale = 1;

        Cursor.visible = false;
        gameOver.SetActive(false);
        isGameOver = false;

        Boma.can_boma = 100;
    }
    public void setGameOver() {
        gameOver.SetActive(true);
    }
    public void updateHealth(float health, float MAX_HEALTH) {
        healthObject.color = new Color(1 - health/MAX_HEALTH, health/MAX_HEALTH, 0, 0.7f);
        StopCoroutine("lerpHealth");
        StartCoroutine(lerpHealth(health, MAX_HEALTH));
        
    }
    IEnumerator lerpHealth(float health, float MAX_HEALTH) {
        Vector3 targetPosition = new Vector3(health/MAX_HEALTH - 1, 0, 0);
        while(Vector3.Distance(targetPosition, healthMask.localPosition) > 0.01) {
            healthMask.localPosition =  Vector3.Lerp(healthMask.localPosition, targetPosition, 3*Time.deltaTime);
            yield return null;
        }
    }
}
