using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    void OnTriggerEnter2D() {
        StartCoroutine(pressedPlay());
    }

    IEnumerator pressedPlay() {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);
    }
}
