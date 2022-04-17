using UnityEngine;

namespace AwesomeCharts {
    public class PieChartClickSelectionBehaviour : MonoBehaviour {

        [SerializeField]
        private PieChart pieChart;

        public PieChart PieChart {
            get { return pieChart; }
            set {
                pieChart = value;
                RegisterClickDelegate ();
            }
        }

        void OnEnable () {
            RegisterClickDelegate ();
        }

        private void RegisterClickDelegate () {
            if (pieChart != null) {
                pieChart.AddEntryClickDelegate (OnEntryClick);
            }
        }

        void OnDisable () {
            if (pieChart != null) {
                pieChart.RemoveEntryClickDelegate (OnEntryClick);
            }
        }

        private void OnEntryClick (int index, PieEntry entry) {
            if (!pieChart.IsEntryAtPositionSelected (index)) {
                pieChart.SelectEntryAtPosition (index);
            } else {
                pieChart.DeselectEntryAtPosition (index);
            }
        }
    }
}