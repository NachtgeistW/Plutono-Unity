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
        // Start is called before the first frame update
        void Start()
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
