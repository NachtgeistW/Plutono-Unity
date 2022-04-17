using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class AxisValue {

        [SerializeField]
        private float min = Defaults.AXIS_MIN_VALUE;
        [SerializeField]
        private float max = Defaults.AXIS_MAX_VALUE;
        [SerializeField]
        private bool minAutoValue = false;
        [SerializeField]
        private bool maxAutoValue = false;

        public float Min {
            get { return min; }
            set { min = value; }
        }

        public float Max {
            get { return max; }
            set { max = value; }
        }

        public bool MinAutoValue {
            get { return minAutoValue; }
            set { minAutoValue = value; }
        }

        public bool MaxAutoValue {
            get { return maxAutoValue; }
            set { maxAutoValue = value; }
        }
    }
}