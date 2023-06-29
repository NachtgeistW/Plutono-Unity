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

using Plutono.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.ChartSelect
{
    public class PrefabButtonChartSelectView : MonoBehaviour
    {
        [SerializeField] private Text level;
        [SerializeField] private Text chartDesigner;
        public Song.ChartDetail ChartOnButton { get; set; }
        private int chartIndex;

        public void SetChartInfo(Song.ChartDetail chart, int index)
        {
            ChartOnButton = chart;
            level.text = chart.level;
            chartDesigner.text = chart.chartDesigner;
            chartIndex = index;
        }

        public void JumpToGameScene()
        {
            SongSelectDataTransformer.SelectedChartIndex = chartIndex;
            EventHandler.CallTransitionEvent("GamePlay");
        }
    }
}
