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

using Assets.Scripts.Model.Plutono;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Views
{
    public class PrefabButtonChartSelectView : MonoBehaviour
    {
        [SerializeField] private Text _level;
        [SerializeField] private Text _chartDesigner;
        public GameChart ChartOnButton { get; set; }

        public void SetChartInfo(GameChart chart)
        {
            ChartOnButton = chart;
            _level.text = chart.level;
            _chartDesigner.text = chart.chartDesigner;
        }

        public void JumpToGameScene()
        {
            GameManager.Instance.gameChart = ChartOnButton;
            SceneManager.LoadScene("GamePlayScene");
        }
    }
}
