using DG.Tweening;
using Plutono.GamePlay;
using Plutono.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.ChartSelect
{
    /// <summary>
    /// Control the events happened in Chart Select Scene.
    /// </summary>
    public class BasicPanel : MonoBehaviour
    {
        [Header("Upper")]
        [SerializeField] private Button buttonMode;
        [SerializeField] private Button buttonSetting;
        [SerializeField] private Button buttonBack;

        [Space(10)] [Header("(Attatch prefab, not script!)�Ա��б��õ�ѡ�����õ�prefab��")]
        [SerializeField] private PrefabButtonChartSelectView prefab;
        [SerializeField] private RectTransform chartListTransform;
        [SerializeField] private Image cover;
        private int songIndex;


        [SerializeField] private RectTransform modePanel;

        [SerializeField] private Text songName;
        [SerializeField] private Text composer;
        [SerializeField] private Text score;

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
        }

        /// <summary>
        /// Instantiate the level and chart designer of charts in chart list using selected prefab.
        /// </summary>
        /// <param name="songDetail"></param>
        private void PopulateChart(Song.SongDetail songDetail)
        {
            var i = 0;
            foreach (var chart in songDetail.chartDetails)
            {
                var newButton = Instantiate(prefab, chartListTransform);
                newButton.SetChartInfo(chart, i);
                i++;
            }
        }
    }
}
