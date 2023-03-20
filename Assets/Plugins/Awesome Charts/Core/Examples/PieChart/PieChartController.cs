using UnityEngine;

namespace AwesomeCharts {
    public class PieChartController : MonoBehaviour {
        public PieChart pieChart;

        private void Start () {
            ConfigChart ();
            AddChartData ();
        }

        private void ConfigChart () {
            // accessing and editing current config           
            pieChart.Config.InnerPadding = 40;
            pieChart.Config.ValueIndicatorFontSize = 18;
            pieChart.Config.ValueIndicatorLineLength = 30;
            pieChart.Config.ValueIndicatorColor = Color.white;
            pieChart.Config.ValueIndicatorVisibility = PieChartConfig.ValueIndicatorVisibilityMode.ONLY_SELECTED;

            // setting new config            
            PieChartConfig config = new PieChartConfig {
                InnerPadding = 40,
                ValueIndicatorFontSize = 18,
                ValueIndicatorLineLength = 30,
                ValueIndicatorColor = Color.white,
                ValueIndicatorVisibility = PieChartConfig.ValueIndicatorVisibilityMode.ONLY_SELECTED
            };
            pieChart.Config = config;
        }

        private void AddChartData () {
            // Create data set for entries            
            PieDataSet set = new PieDataSet ();

            // Add entries to data set             
            set.AddEntry (new PieEntry (20, "Entry 1", Color.red));
            set.AddEntry (new PieEntry (30, "Entry 2", Color.green));
            set.AddEntry (new PieEntry (25, "Entry 3", Color.blue));

            // Add data set to chart data            
            pieChart.GetChartData ().DataSet = set;

            // Refresh chart after data change            
            pieChart.SetDirty ();
        }
    }
}