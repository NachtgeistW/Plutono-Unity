/*
 * History
 *      2021.04.09  CREATE.
 *      2021.10.3 Rewrite comments
 */

using Assets.Scripts.Views;

using Model.Plutono;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Views;

namespace Controller
{
    /// <summary>
    /// Control the events happened in Chart Select Scene.
    /// </summary>
    public class ChartSelectController : MonoBehaviour
    {
        [Header("(放prefab不是script！)旁边列表用的选谱面用的prefab。")]
        [SerializeField] private PrefabButtonChartSelectView _prefab;
        [SerializeField] private ChartSelectView _view;
        [SerializeField] private Image cover;

        private void Start()
        {
            var pack = GameManager.Instance.SongInfo;
            cover.sprite = pack.Cover;
            SetSongInfo(pack);
            PopulateChart(pack);
        }

        /// <summary>
        /// Set the name and composer of this song.
        /// </summary>
        /// <param name="packInfo"></param>
        public void SetSongInfo(SongInfo packInfo)
        {
            _view.SetSongName(packInfo.SongName);
            _view.SetComposer(packInfo.Composer);
        }

        /// <summary>
        /// Instantiate the level and chart designer of charts in chart list using selected prefab.
        /// </summary>
        /// <param name="packInfo"></param>
        public void PopulateChart(SongInfo packInfo)
        {
            foreach (var chart in packInfo.Charts)
            {
                var newButton = Instantiate(_prefab, transform);
                newButton.SetChartInfo(chart);
            }
        }

        public void OnBack()
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }
}
