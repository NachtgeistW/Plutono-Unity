using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class BarEntry : Entry {

        [SerializeField]
        private long position;

        public BarEntry() : base() {
            this.position = 0;
        }

        public BarEntry(long position, float value) : base(value) {
            this.position = position;
        }

        public long Position {
            get { return position; }
            set { position = value; }
        }
    }
}