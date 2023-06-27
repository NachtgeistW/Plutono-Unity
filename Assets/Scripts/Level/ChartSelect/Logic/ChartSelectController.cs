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
        [SerializeField] private PrefabButtonChartSelectView prefab;
        [SerializeField] private ChartSelectView view;
        [SerializeField] private Image cover;
        private int songIndex;

        private void Start()
        {
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            var pack = FileManager.Instance.songSourceList[songIndex];
            cover.sprite = pack.cover;
            SetSongInfo(pack);
            PopulateChart(pack);
        }

        /// <summary>
        /// Set the name and composer of this song.
        /// </summary>
        /// <param name="packInfo"></param>
        public void SetSongInfo(Song.SongDetail songDetail)
        {
            view.SetSongName(songDetail.songName);
            view.SetComposer(songDetail.composer);
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
                var newButton = Instantiate(prefab, transform);
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
