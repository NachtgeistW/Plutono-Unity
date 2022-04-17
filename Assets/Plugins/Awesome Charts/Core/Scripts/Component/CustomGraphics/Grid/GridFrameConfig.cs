using System;
using UnityEngine;

namespace AwesomeCharts {
    [Serializable]
    public class GridFrameConfig {

        [SerializeField]
        private bool drawLeftLine = true;
        [SerializeField]
        private bool drawRightLine = true;
        [SerializeField]
        private bool drawTopLine = true;
        [SerializeField]
        private bool drawBottomLine = true;
        [SerializeField]
        private GridLineConfig linesConfig = new GridLineConfig();

        public bool DrawLeftLine {
            get { return drawLeftLine; }
            set { drawLeftLine = value; }
        }

        public bool DrawRightLine {
            get { return drawRightLine; }
            set { drawRightLine = value; }
        }

        public bool DrawTopLine {
            get { return drawTopLine; }
            set { drawTopLine = value; }
        }

        public bool DrawBottomLine {
            get { return drawBottomLine; }
            set { drawBottomLine = value; }
        }

        public GridLineConfig LinesConfig {
            get { return linesConfig; }
            set { linesConfig = value; }
        }
    }
}