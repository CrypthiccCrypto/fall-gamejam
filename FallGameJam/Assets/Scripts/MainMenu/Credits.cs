using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] MainMenuManager mainMenuManager;
    void OnTriggerEnter2D() {
        StartCoroutine(pressedCredits());
    }

    IEnumerator pressedCredits() {
        yield return new WaitForSeconds(0.1f);
        mainMenuManager.credits();
    }
}
