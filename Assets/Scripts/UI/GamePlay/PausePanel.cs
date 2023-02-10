namespace Plutono.UI
{
    using UnityEngine;
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        private void OnEnable()
        {
            EventHandler.GamePauseEvent += OnGamePauseEvent;
            EventHandler.GameResumeEvent += OnGameResumeEvent;
        }

        private void OnDisable()
        {
            EventHandler.GamePauseEvent -= OnGamePauseEvent;
            EventHandler.GameResumeEvent -= OnGameResumeEvent;
        }

        private void OnGamePauseEvent()
        {
            pausePanel.SetActive(true);
        }

        private void OnGameResumeEvent()
        {
            pausePanel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            pausePanel.SetActive(false);
        }
    }
}