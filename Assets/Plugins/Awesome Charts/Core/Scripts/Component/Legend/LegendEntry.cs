using System;
using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class LegendEntry {

        [SerializeField]
        private String title;
        [SerializeField]
        private Color color;

        public LegendEntry() {
            title = "";
            color = Color.black;
        }

        public LegendEntry(string title, Color color) {
            this.title = title;
            this.color = color;
        }

        public String Title {
            get { return title; }
            set { title = value; }
        }

        public Color Color {
            get { return color; }
            set { color = value; }
        }
    }
}
