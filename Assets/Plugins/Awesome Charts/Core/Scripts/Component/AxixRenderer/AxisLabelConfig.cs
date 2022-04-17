using System;
using UnityEngine;

namespace AwesomeCharts {

    [Serializable]
    public class AxisLabelConfig {
        [SerializeField]
        private int labelSize = Defaults.AXIS_LABEL_SIZE;
        [SerializeField]
        private Color labelColor = Defaults.AXIS_LABEL_COLOR;
        [SerializeField]
        private float labelMargin = Defaults.AXIS_LABEL_MARGIN;

        private Font defaultFont;

        public int LabelSize {
            get { return labelSize; }
            set { labelSize = value; }
        }

        public Color LabelColor {
            get { return labelColor; }
            set { labelColor = value; }
        }

        public float LabelMargin {
            get { return labelMargin; }
            set { labelMargin = value; }
        }
    }
}