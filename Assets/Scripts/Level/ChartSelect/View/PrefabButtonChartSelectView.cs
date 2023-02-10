/*
 * class PrefabCoverView -- Store the object and function that change UI on the Prefab ButtonChartSelect.
 *      _level: private Text, the level of this chart attached to this button.
 *      _chartDesigner: private Text, the chart designer of this chart attached to this button.
 *      ChartOnButton: public GameChart, property, the chart info attached to this button.
 *
 * Function
 *      void::SetChartInfo -- Set the chart, level and chart designer to a prefab.
 *      void::JumpToChartSelectScene -- Jump to game playing scene.
 *
 * History
 *      2021.04.09  CREATE.
 */

using Assets.Scripts.GamePlay;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plutono.Level.ChartSelect
{
    public class PrefabButtonChartSelectView : MonoBehaviour
    {
        [SerializeField] private Text _level;
        [SerializeField] private Text _chartDesigner;
        public Song.ChartDetail ChartOnButton { get; set; }
        private int chartIndex;

        public void SetChartInfo(Song.ChartDetail chart, int chartIndex)
        {
            ChartOnButton = chart;
            _level.text = chart.level;
            _chartDesigner.text = chart.chartDesigner;
            this.chartIndex = chartIndex;
        }

        public void JumpToGameScene()
        {
            SongSelectDataTransformer.SelectedChartIndex = chartIndex;
            EventHandler.CallTransitionEvent("GamePlay");
        }
    }
}
