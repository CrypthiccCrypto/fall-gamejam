using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int SCORE;
    private Text text;
    void Start() {
        SCORE = 0;
        text = this.GetComponent<Text>();
    }
    void Update() {
        text.text = SCORE.ToString();
    }
}
