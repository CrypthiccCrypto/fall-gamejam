using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickMenu : MonoBehaviour
{   
    bool isOver;
    void OnMouseOver() {
        isOver = true;
    }

    void OnMouseExit() {
        isOver = false;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(isOver)
                SceneManager.LoadScene(0);
        }
    }
}
