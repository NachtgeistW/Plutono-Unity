﻿using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class ResultController : MonoBehaviour
    {
        public Text textSongName;
        public Text textScore;
        public Text textPerfect;
        public Text textGood;
        public Text textBad;
        public Text textMiss;
        public Image cover;

        private void Start()
        {
            textSongName.text = GameManager.Instance.SongInfo.SongName;
            textScore.text = Convert.ToString(GameManager.Instance.score) + " + "
                + Convert.ToString(GameManager.Instance.bonus * 100000 / 1024);
            textPerfect.text = "P " + Convert.ToString(GameManager.Instance.pCount);
            textGood.text = "G " + Convert.ToString(GameManager.Instance.gCount);
            textBad.text = "B " + Convert.ToString(GameManager.Instance.bCount);
            textMiss.text = "M " + Convert.ToString(GameManager.Instance.mCount);
            cover.sprite = GameManager.Instance.SongInfo.Cover;
        }

        public void OnRetry()
        {
            SceneManager.LoadScene("GamePlayScene");
        }

        public void OnContinue()
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }
}
