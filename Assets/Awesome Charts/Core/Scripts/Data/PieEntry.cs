using System;
using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class PieEntry : Entry {

        [SerializeField]
        private Color color = Color.white;
        [SerializeField]
        private String label = "";

        public PieEntry() : base() {
        }

        public PieEntry(float value, string label, Color color): base(value) {
            this.label = label;
            this.color = color;
        }

        public Color Color {
            get { return color; }
            set { color = value; }
        }

        public String Label {
            get { return label; }
            set { label = value; }
        }
    }
}
