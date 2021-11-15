using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class LineEntry : Entry {

        [SerializeField]
        private float position;

        public LineEntry() : base() {
            this.position = 0f;
        }

        public LineEntry(float position, float value) : base(value) {
            this.position = position;
        }

        public float Position {
            get { return position; }
            set { position = value; }
        }

        override public float Value {
            get { return value; }
            set { this.value = value; }
        }
    }
}