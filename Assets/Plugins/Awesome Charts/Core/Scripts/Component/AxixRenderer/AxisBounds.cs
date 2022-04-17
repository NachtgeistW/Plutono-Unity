using System;
using UnityEngine;

namespace AwesomeCharts {
    [Serializable]
    public class AxisBounds {
        [SerializeField]
        private float xMin;
        [SerializeField]
        private float xMax;
        [SerializeField]
        private float yMin;
        [SerializeField]
        private float yMax;

        public float XMin {
            get { return xMin; }
            set { xMin = value; }
        }

        public float XMax {
            get { return xMax; }
            set { xMax = value; }
        }

        public float YMin {
            get { return yMin; }
            set { yMin = value; }
        }

        public float YMax {
            get { return yMax; }
            set { yMax = value; }
        }

        public AxisBounds(float xMin, float xMax, float yMin, float yMax) {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }
    }
}
