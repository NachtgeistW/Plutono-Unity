/*
 * class UIController -- Control the UI on game.
 *
 * History
 *      2021.10.16  CREATE and move the function into here.
 */

using System;
using Assets.Scripts.Controller.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Game
{
    public class UIController : MonoBehaviour
    {
        [Header("-Text-")]

        [SerializeField] private Text textReady;

        [SerializeField] private Text textSongName;
        [SerializeField] private Text textScore;
        [SerializeField] private Text textLevel;
        [SerializeField] private Text textMode;

        [SerializeField] private Text textCombo;
        [SerializeField] private Text textComboCount;

        [SerializeField] private Text textEarly;
        [SerializeField] private Text textLate;

        private const float EarlyAndLateLifetime = 0.3f;
        private float earlyShowtime;
        private float lateShowtime;
        private bool isEarlyShown;
        private bool isLateShown;

        public Slider timeSlider;
        public void InitializeUi(string songName, string level, float musicLength)
        {
            textSongName.text = songName;
            textScore.text = Convert.ToString(0);
            textLevel.text = "Lv." + level;
            SetMode(GameMode.Arbo);

            HideCombo();
            textEarly.enabled = false;
            textLate.enabled = false;
            timeSlider.maxValue = musicLength;
        }

        public void OnGameUpdate(float time, GameStatus status)
        {
            timeSlider.value = time; 
            SetScoreText(status.BasicScore);

            if (status.Combo > 5)
                ShowCombo(status.Combo);
            else
                HideCombo();

            HideEarly();
            HideLate();
        }

        public void SetMode(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Stelo:
                    textMode.text = "Stelo";
                    break;
                case GameMode.Arbo:
                    textMode.text = "Arbo";
                    break;
                case GameMode.Floro:
                    textMode.text = "Floro";
                    break;
                case GameMode.Persona:
                    textMode.text = "Persona";
                    break;
                case GameMode.Ekzerco:
                    textMode.text = "Ekzerco";
                    break;
            }
        }

        public void HideCombo()
        {
            textCombo.enabled = false;
            textComboCount.enabled = false;
        }

        public void ShowCombo(int comboCount)
        {
            textCombo.enabled = true;
            textComboCount.enabled = true;
            textComboCount.text = Convert.ToString(comboCount);
        }

        public void HideEarly()
        {
            if (!isEarlyShown) return;
            earlyShowtime += Time.unscaledDeltaTime;
            if (!(earlyShowtime > EarlyAndLateLifetime)) return;
            textEarly.enabled = false;
            isEarlyShown = false;
        }

        public void ShowEarly()
        {
            isEarlyShown = true;
            textEarly.enabled = true;
            earlyShowtime = 0;
        }

        public void HideLate()
        {
            if (!isLateShown) return;
            lateShowtime += Time.unscaledDeltaTime;
            if (!(lateShowtime > EarlyAndLateLifetime)) return;
            textLate.enabled = false;
            isLateShown = false;
        }

        public void ShowLate()
        {
            isLateShown = true;
            textLate.enabled = true;
            lateShowtime = 0;
        }
        public void SetScoreText(int score)
        {
            textScore.text = Convert.ToString(score);
        }
    }
}
