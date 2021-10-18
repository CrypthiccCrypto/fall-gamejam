using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    void OnTriggerEnter2D() {
        StartCoroutine(pressedQuit());
    }

    IEnumerator pressedQuit() {
        yield return new WaitForSeconds(0.1f);
        Application.Quit();
    }
}
