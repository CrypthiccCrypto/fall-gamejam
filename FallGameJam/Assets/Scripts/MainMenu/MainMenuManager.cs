using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject ruleMenu;
    [SerializeField] GameObject creditMenu;
    [SerializeField] GameObject rulePlayer;
    void Start()
    {
        Cursor.visible = false;
        mainMenu.SetActive(true);
        ruleMenu.SetActive(false);
        creditMenu.SetActive(false);
        
        Time.timeScale = 1;
    }

    public void rules() {
        mainMenu.SetActive(false);
        ruleMenu.SetActive(true);
        rulePlayer.GetComponent<RulePlayer>().enabled = true;
    }

    public void credits() {
        mainMenu.SetActive(false);
        creditMenu.SetActive(true);
    }

    public void returnMenuCredit() {
        mainMenu.SetActive(true);
        creditMenu.SetActive(false);
    }

    public void returnMenu() {
        StartCoroutine(returnMenuCoroutine());
    }

    IEnumerator returnMenuCoroutine() {
        rulePlayer.GetComponent<RulePlayer>().enabled = false;
        float timer = 0.0f;

        while(Vector3.Distance(rulePlayer.transform.position, Vector3.zero) > 0.01 && timer < 0.7f) {
            rulePlayer.transform.position = Vector3.Lerp(rulePlayer.transform.position, Vector3.zero, 6*Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        
        mainMenu.SetActive(true);
        ruleMenu.SetActive(false);
    }
}
