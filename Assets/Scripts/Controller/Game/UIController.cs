/*
 * class UIController -- Control the UI on game.
 *
 * History
 *      2021.10.16  CREATE and move the function into here.
 */

using System;
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

        public void InitializeUI(string songName, string level)
        {
            textSongName.text = songName;
            textScore.text = Convert.ToString(0);
            textLevel.text = "Lv." + level;
            //_comboCount = 0;

            HideCombo();
            textEarly.enabled = false;
            textLate.enabled = false;
        }

        public void HideCombo()
        {
            textCombo.enabled = false;
            textComboCount.enabled = false;
        }

        public void ShowCombo(uint comboCount)
        {
            textCombo.enabled = true;
            textComboCount.enabled = true;
            textComboCount.text = Convert.ToString(comboCount);
        }

        public void HideEarly()
        {
            if (!isEarlyShown) return;
            earlyShowtime += Time.deltaTime;
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
            lateShowtime += Time.deltaTime;
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
        public void ChangeScoreText(int score)
        {
            textScore.text = Convert.ToString(score);
        }
    }
}
