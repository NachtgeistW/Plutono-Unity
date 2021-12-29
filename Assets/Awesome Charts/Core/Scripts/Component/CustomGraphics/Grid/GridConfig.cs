using System;
using UnityEngine;

namespace AwesomeCharts {
    [Serializable]
    public class GridConfig {
        [SerializeField]
        private int verticalLinesCount = Defaults.AXIS_LINES_COUNT;
        [SerializeField]
        private int horizontalLinesCount = Defaults.AXIS_LINES_COUNT;
        [SerializeField]
        private GridLineConfig verticalLinesConfig = new GridLineConfig ();
        [SerializeField]
        private GridLineConfig horizontalLinesConfig = new GridLineConfig ();

        public int VerticalLinesCount {
            get { return verticalLinesCount; }
            set {verticalLinesCount = value;}
        }

        public int HorizontalLinesCount {
            get { return horizontalLinesCount; }
            set { horizontalLinesCount = value; }
        }

        public GridLineConfig VerticalLinesConfig {
            get { return verticalLinesConfig; }
            set { verticalLinesConfig = value; }
        }

        public GridLineConfig HorizontalLinesConfig {
            get { return horizontalLinesConfig; }
            set { horizontalLinesConfig = value; }
        }
    }
}