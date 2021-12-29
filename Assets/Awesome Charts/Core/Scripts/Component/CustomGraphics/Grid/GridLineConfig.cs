using System;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class GridLineConfig {

        [SerializeField]
        private int thickness = Defaults.AXIS_LINE_THICKNESS;
        [SerializeField]
        private Color color = Defaults.AXIS_LINE_COLOR;
        [SerializeField]
        private bool dashed = false;
        [SerializeField]
        private int dashLength = 0;
        [SerializeField]
        private int dashSpacing = 0;

        public int Thickness {
            get { return thickness; }
            set { thickness = value; }
        }

        public Color Color {
            get { return color; }
            set { color = value; }
        }

        public bool Dashed {
            get { return dashed; }
            set { dashed = value; }
        }

        public int DashLenght {
            get { return dashLength; }
            set { dashLength = value; }
        }

        public int DashSpacing {
            get { return dashSpacing; }
            set { dashSpacing = value; }
        }

        public bool ShouldDrawDashedLines () {
            return dashed && dashLength > 0 && dashSpacing > 0;
        }
    }
}