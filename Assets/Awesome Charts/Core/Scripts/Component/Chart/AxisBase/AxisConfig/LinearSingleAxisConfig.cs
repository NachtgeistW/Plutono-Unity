using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class LinearSingleAxisConfig : SingleAxisConfig {

        [SerializeField]
        private int labelsCount = Defaults.AXIS_LABELS_COUNT;
        [SerializeField]
        private AxisLabelGravity labelsAlignment = AxisLabelGravity.START;
        [SerializeField]
        private bool drawStartValue = true;
        [SerializeField]
        private bool drawEndValue = true;
        [SerializeField]
        private BasicValueFormatterConfig valueFormatterConfig = new BasicValueFormatterConfig ();

        public int LabelsCount {
            get { return labelsCount; }
            set { labelsCount = value; }
        }

        public bool DrawStartValue {
            get { return drawStartValue; }
            set { drawStartValue = value; }
        }

        public bool DrawEndValue {
            get { return drawEndValue; }
            set { drawEndValue = value; }
        }

        public AxisLabelGravity LabelsAlignment {
            get { return labelsAlignment; }
            set { labelsAlignment = value; }
        }

        public BasicValueFormatterConfig ValueFormatterConfig {
            get { return valueFormatterConfig; }
            set { valueFormatterConfig = value; }
        }
    }
}