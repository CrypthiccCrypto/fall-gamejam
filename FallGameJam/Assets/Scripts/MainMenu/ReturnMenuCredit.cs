using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnMenuCredit : MonoBehaviour
{
    [SerializeField] MainMenuManager mainMenuManager;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Bullet")
            StartCoroutine(pressedRules());
    }

    IEnumerator pressedRules() {
        yield return new WaitForSeconds(0.1f);
        mainMenuManager.returnMenuCredit();
    }
}
