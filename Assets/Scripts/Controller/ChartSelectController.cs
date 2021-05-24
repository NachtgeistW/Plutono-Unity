/*
 * class SongSelectSceneController -- Control the events happened in Chart Select Scene.
 *
 * Function
 *      void::SetSongInfo -- Set the name and composer of this song.
 *      void::PopulateChart -- Instantiate the level and chart designer of charts in chart list using selected prefab.
 *
 * History
 *      2020.04.09  CREATE.
 */

using Assets.Scripts.Views;
using Model.Plutono;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Views;

namespace Controller
{
    public class ChartSelectController : MonoBehaviour
    {
        [Header("(放prefab不是script！)旁边列表用的选谱面用的prefab。")]
        [SerializeField]private PrefabButtonChartSelectView _prefab;
        [SerializeField]private ChartSelectView _view;
        [SerializeField] private Image cover;

        private void Start()
        {
            var pack = GameManager.Instance.packInfo;
            cover.sprite = pack.cover;
            SetSongInfo(pack);
            PopulateChart(pack);
        }
        public void SetSongInfo(PackInfo packInfo)
        {
            _view.SetSongName(packInfo.songName);
            _view.SetComposer(packInfo.composer);
        }

        public void PopulateChart(PackInfo packInfo)
        {
            foreach (var chart in packInfo.charts)
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
