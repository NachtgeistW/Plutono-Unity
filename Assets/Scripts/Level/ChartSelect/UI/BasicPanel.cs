using System;
using System.Collections.Generic;
using DG.Tweening;
using Plutono.GamePlay;
using Plutono.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.ChartSelect
{
    /// <summary>
    /// Control the events happened in Chart Select Scene.
    /// </summary>
    public class BasicPanel : MonoBehaviour
    {
        [Header("Basic Panel")]
        [SerializeField] private Button buttonMode;
        [SerializeField] private Button buttonSetting;
        [SerializeField] private Button buttonBack;
        [SerializeField] private Button buttonStart;

        [Tooltip("(Attach prefab, not script!)")]
        [SerializeField] private PrefabButtonChartSelectView prefab;
        [SerializeField] private RectTransform chartListTransform;
        [SerializeField] private Image cover;
        private int songIndex;
        private readonly List<PrefabButtonChartSelectView> charts = new ();

        [SerializeField] private Text songName;
        [SerializeField] private Text composer;
        [SerializeField] private Text score;

        [Space(10)]
        [Header("Mode Panel")]
        [SerializeField] private RectTransform modePanel;

        private void Start()
        {
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            var songDetail = FileManager.Instance.songSourceList[songIndex];
            cover.sprite = songDetail.cover;
            PopulateChart(songDetail);

            // Set the name and composer of this song.
            songName.text = songDetail.songName;
            composer.text = songDetail.composer;

            buttonMode.onClick.AddListener(() =>
                modePanel.DOAnchorPosY(0, 0.5f).SetEase(Ease.InOutSine)
                );
            buttonBack.onClick.AddListener(() =>
                EventHandler.CallTransitionEvent("SongSelect")
                );
            buttonStart.onClick.AddListener(() =>
                    EventHandler.CallTransitionEvent("GamePlay")
                );
        }

        /// <summary>
        /// Instantiate the level and chart designer of charts in chart list using selected prefab.
        /// </summary>
        private void PopulateChart(Song.SongDetail songDetail)
        {
            var i = 0;
            foreach (var chart in songDetail.chartDetails)
            {
                var newButton = Instantiate(prefab, chartListTransform);
                newButton.SetChartInfo(chart, i);
                newButton.Button.onClick.AddListener(() => OnSelectChart(newButton.ChartIndex));
                charts.Add(newButton);
                i++;
            }
        }

        private void OnSelectChart(int chartIndex)
        {
            SongSelectDataTransformer.SelectedChartIndex = chartIndex;
            charts.ForEach(chart => chart.SetSelected(false));
            charts[chartIndex].SetSelected(true);

            var id = charts[chartIndex].ChartOnButton.id;
            var mode = SongSelectDataTransformer.GameMode;
            score.text = GameData.PlayerScores.Instance.Load(id, mode).ToString();

            buttonStart.interactable = true;
        }
    }
}
