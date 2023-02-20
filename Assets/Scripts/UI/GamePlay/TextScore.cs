using Plutono.GamePlay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScore : MonoBehaviour
{
    public TMP_Text scoreText;
    public GamePlayController gamePlayController;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0 + 0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gamePlayController.Status.BasicScore.ToString() + " + " + gamePlayController.Status.ComboScore.ToString();
    }

}
