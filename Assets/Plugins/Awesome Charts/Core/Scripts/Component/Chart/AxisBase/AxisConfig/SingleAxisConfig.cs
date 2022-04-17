using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class SingleAxisConfig {

        [SerializeField]
        private AxisLabelConfig labelsConfig = new AxisLabelConfig ();
        [SerializeField]
        private AxisValue bounds = new AxisValue ();
        [SerializeField]
        private ChartLabel axisLabelPrefab;

        public AxisLabelConfig LabelsConfig {
            get { return labelsConfig; }
            set { labelsConfig = value; }
        }

        public AxisValue Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        public ChartLabel AxisLabelPrefab {
            get { return axisLabelPrefab; }
            set { axisLabelPrefab = value; }
        }
    }
}