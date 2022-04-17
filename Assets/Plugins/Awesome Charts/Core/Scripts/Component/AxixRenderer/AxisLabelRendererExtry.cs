using System;
using UnityEngine;

namespace AwesomeCharts {
    [Serializable]
    public class AxisLabelRendererExtry {

        [SerializeField]
        private string text;
        [SerializeField]
        private float positionOnAxis;
        [SerializeField]
        private AxisOrientation orientation;
        [SerializeField]
        private AxisLabelGravity gravity;

        public string Text {
            set { text = value; }
            get { return text; }
        }

        public float PositionOnAxis {
            set { positionOnAxis = value; }
            get { return positionOnAxis; }
        }

        public AxisOrientation Orientation {
            set { orientation = value; }
            get { return orientation; }
        }

        public AxisLabelGravity Gravity {
            set { gravity = value; }
            get { return gravity; }
        }
    }
}