using Plutono.GamePlay;  
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Plutono.Level.ChartSelect
{
    public class ModePanel : MonoBehaviour
    {

        [SerializeField] private RectTransform modePanel;

        [Space(10)] 
        [Header("Buttons")]
        [SerializeField] private Button exitButton;

        [Space(10)]
        [Header("Elements")]
        [SerializeField] private TMP_Dropdown modeDropdown;
        [SerializeField] private TMP_Dropdown speedDropdown;

        [Space(10)]
        [Header("Basic Panel")]
        [SerializeField] private Text modeText;

        private void Start()
        {
            // Set default dropdown values
            modeDropdown.value = (int)SongSelectDataTransformer.GameMode;
            speedDropdown.value = SongSelectDataTransformer.ChartPlaySpeed switch
            {
                0.5f => 0,
                5.0f => 1,
                5.5f => 2,
                6.0f => 3,
                6.5f => 4,
                _ => 2,
            };
            modeDropdown.onValueChanged.AddListener(value =>
            {
                SongSelectDataTransformer.GameMode = (GameMode)value;
                modeText.text = Convert.ToString(SongSelectDataTransformer.GameMode);
            });
            speedDropdown.onValueChanged.AddListener(value =>
            {
                SongSelectDataTransformer.ChartPlaySpeed = value switch
                {
                    0 => 0.5f,
                    1 => 5.0f,
                    2 => 5.5f,
                    3 => 6.0f,
                    4 => 6.5f,
                    _ => 5.5f,
                };
            });

            exitButton.onClick.AddListener(() =>
            {
                modePanel.DOAnchorPosY(500, 0.5f).SetEase(Ease.InOutSine);
            });
        }
    }
}