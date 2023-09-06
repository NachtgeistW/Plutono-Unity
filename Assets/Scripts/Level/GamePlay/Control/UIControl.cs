using Plutono.UI;
using System;
using System.Collections;
using Plutono.GamePlay;
using Plutono.GamePlay.Notes;
using Plutono.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem.XR;
using LeTai.Asset.TranslucentImage;

namespace Plutono.Level.GamePlay.Control
{
    public class UIControl : MonoBehaviour
    {
        [SerializeField] private TranslucentImageSource source;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text modeText;
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private Text textCountdown;
        [SerializeField] private Slider slider;
        [SerializeField] private Button pauseButton;

        [Space(10)]
        [Header("Pause Panel")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button chartSelectButton;
        [SerializeField] private Button songSelectButton;

        #region UnityEvent

        private void OnEnable()
        {
            EventCenter.AddListener<NoteClearEvent<BlankNote>>(_ => { OnNoteCollect(); });
            EventCenter.AddListener<NoteClearEvent<PianoNote>>(_ => { OnNoteCollect(); });
            EventCenter.AddListener<NoteClearEvent<SlideNote>>(_ => { OnNoteCollect(); });
            EventCenter.AddListener<NoteMissEvent<BlankNote>>(_ => { OnNoteCollect(); });
            EventCenter.AddListener<NoteMissEvent<PianoNote>>(_ => { OnNoteCollect(); });
            EventCenter.AddListener<NoteMissEvent<SlideNote>>(_ => { OnNoteCollect(); });
        }

        private void OnDisable()
        {
            EventCenter.RemoveAllListener<NoteClearEvent<BlankNote>>();
            EventCenter.RemoveAllListener<NoteClearEvent<PianoNote>>();
            EventCenter.RemoveAllListener<NoteClearEvent<SlideNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<BlankNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<PianoNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<SlideNote>>();
        }

        private void Start()
        {
            OnGamePrepare();
        }

        private void Update()
        {
            slider.value = GamePlayController.Instance.musicSource.time;
        }

        #endregion

        private void OnGamePrepare()
        {
            comboText.text = "0";
            scoreText.text = "0 + 0";
            modeText.text = GamePlayController.Instance.Status.Mode.ToString();
            slider.maxValue = GamePlayController.Instance.musicSource.clip.length;

            pauseButton.onClick.AddListener(OnGamePause);

            resumeButton.onClick.AddListener(OnGameResume);
            restartButton.onClick.AddListener(OnGameRestart);
            chartSelectButton.onClick.AddListener(() => { EventHandler.CallTransitionEvent("ChartSelect"); });
            songSelectButton.onClick.AddListener(() => { EventHandler.CallTransitionEvent("SongSelect"); });

            StartCoroutine(PrepareCountdown());
        }

        private void OnGamePause()
        {
            EventCenter.Broadcast(new GamePauseEvent());

            pausePanel.SetActive(true);
            pauseButton.interactable = false;

            source.enabled = true;
        }

        private void OnGameResume()
        {
            pausePanel.SetActive(false);
            pauseButton.interactable = true;

            StartCoroutine(ResumeCountdown());

            source.enabled = false;
        }

        private void OnGameRestart()
        {
            EventCenter.Broadcast(new GamePrepareEvent());
            pausePanel.SetActive(false);
            StartCoroutine(PrepareCountdown());
            source.enabled = false;
        }

        private void OnNoteCollect()
        {
            comboText.text = GamePlayController.Instance.Status.Combo.ToString();
            scoreText.text = GamePlayController.Instance.Status.BasicScore + " + " +
                             GamePlayController.Instance.Status.ComboScore;
        }

        public IEnumerator PrepareCountdown()
        {
            yield return new WaitForSecondsRealtime(1.0f);

            textCountdown.text = "Start";
            yield return new WaitForSecondsRealtime(1.0f);
            textCountdown.gameObject.SetActive(false);
            EventCenter.Broadcast(new GameStartEvent());
        }

        public IEnumerator ResumeCountdown()
        {
            textCountdown.gameObject.SetActive(true);
            textCountdown.text = "Music";
            yield return new WaitForSecondsRealtime(1.0f);

            textCountdown.text = "Resume";
            yield return new WaitForSecondsRealtime(1.0f);
            textCountdown.gameObject.SetActive(false);

            Debug.Log("UIControl Broadcast GameResume");
            EventCenter.Broadcast(new GameResumeEvent());
        }
    }
}