using Plutono.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    public class Silder : MonoBehaviour
    {
        [SerializeField] GamePlayController controller;
        [SerializeField] Slider slider;
        
        private void OnEnable()
        {
            EventHandler.GameStartEvent += OnGameStartEvent;
        }

        private void OnDisable()
        {
            EventHandler.GameStartEvent -= OnGameStartEvent;
        }

        void OnGameStartEvent()
        {
            slider.maxValue = controller.musicSource.clip.length;
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = controller.musicSource.time;
        }
    }
}
