/*
 * History
 *      2021.04.09  CREATE.
 *      2021.10.3 Rewrite comments
 */

using Plutono.GamePlay;
using Plutono.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.ChartSelect
{
    /// <summary>
    /// Control the events happened in Chart Select Scene.
    /// </summary>
    public class ChartSelectController : MonoBehaviour
    {
        [Header("(Attatch prefab, not script!)�Ա��б��õ�ѡ�����õ�prefab��")]
        [SerializeField] private PrefabButtonChartSelectView _prefab;
        [SerializeField] private ChartSelectView _view;
        [SerializeField] private Image cover;
        private int songIndex;

        private void Start()
        {
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            var pack = FileManager.Instance.songSourceList[songIndex];
            cover.sprite = pack.Cover;
            SetSongInfo(pack);
            PopulateChart(pack);
        }

        /// <summary>
        /// Set the name and composer of this song.
        /// </summary>
        /// <param name="packInfo"></param>
        public void SetSongInfo(Song.SongDetail songDetail)
        {
            _view.SetSongName(songDetail.songName);
            _view.SetComposer(songDetail.composer);
        }

        /// <summary>
        /// Instantiate the level and chart designer of charts in chart list using selected prefab.
        /// </summary>
        /// <param name="packInfo"></param>
        public void PopulateChart(Song.SongDetail songDetail)
        {
            int i = 0;
            foreach (var chart in songDetail.chartDetails)
            {
                var newButton = Instantiate(_prefab, transform);
                newButton.SetChartInfo(chart, i);
                i++;
            }
        }

        public void OnBack()
        {
            EventHandler.CallTransitionEvent("SongSelect");
        }
    }
}
