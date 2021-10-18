using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour
{
    [SerializeField] MainMenuManager mainMenuManager;
    void OnTriggerEnter2D() {
        StartCoroutine(pressedRules());
    }

    IEnumerator pressedRules() {
        yield return new WaitForSeconds(0.1f);
        mainMenuManager.rules();
    }
}
