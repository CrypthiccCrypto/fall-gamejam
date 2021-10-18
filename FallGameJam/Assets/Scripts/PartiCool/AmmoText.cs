using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private GunReload gunReload;
    [SerializeField] GameObject parent;
    private Text text;
    void Start() {
        text = this.GetComponent<Text>();
    }
    void Update() {
        text.text = gunReload.curr_Mag + " / " + gunReload.MAX_MAG_SIZE;
        if(GameManager.isGameOver)
            parent.SetActive(false);
    }
}
