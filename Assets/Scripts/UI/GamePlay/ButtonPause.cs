/* 
 * History:
 *      2023.01.10  CREATED
 */

using Plutono.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    /// <summary>
    /// Contro the behaivour on pause bottom
    /// </summary>
    public class ButtonPause : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private GamePlayController controller;

        private void OnEnable()
        {
            EventHandler.GameClearEvent += DeactivateButton;
            EventHandler.GameFailEvent += DeactivateButton;
            EventHandler.GameResumeEvent += ActivateButton;
        }

        private void OnDisable()
        {
            EventHandler.GameClearEvent -= DeactivateButton;
            EventHandler.GameFailEvent -= DeactivateButton;
            EventHandler.GameResumeEvent -= ActivateButton;
        }

        private void Start()
        {
            pauseButton.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            //If the game is playing (IsPaused == false), call the pause event;
                EventHandler.CallGamePauseEvent();
                pauseButton.interactable = false;
        }

        private void DeactivateButton()
        {
            pauseButton.interactable = false;
            pauseButton.onClick.RemoveAllListeners();
        }
        private void ActivateButton()
        {
            pauseButton.interactable = true;
        }
    }
}