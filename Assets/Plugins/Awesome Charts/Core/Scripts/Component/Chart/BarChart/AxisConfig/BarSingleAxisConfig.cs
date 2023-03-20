using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class BarSingleAxisConfig : SingleAxisConfig {

        [SerializeField]
        private BarAxisValueFormatterConfig valueFormatterConfig = new BarAxisValueFormatterConfig ();
        [SerializeField]
        private AxisLabelGravity labelsAlignment = AxisLabelGravity.START;

        public BarSingleAxisConfig(){
            Bounds.MinAutoValue = true;
            Bounds.MaxAutoValue = true;
        }

        public AxisLabelGravity LabelsAlignment {
            get { return labelsAlignment; }
            set { labelsAlignment = value; }
        }

        public BarAxisValueFormatterConfig ValueFormatterConfig {
            get { return valueFormatterConfig; }
            set { valueFormatterConfig = value; }
        }
    }
}