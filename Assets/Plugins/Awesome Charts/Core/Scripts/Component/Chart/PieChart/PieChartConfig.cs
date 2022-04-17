using System;
using UnityEngine;

namespace AwesomeCharts {

    [Serializable]
    public class PieChartConfig {

        public enum ValueIndicatorVisibilityMode {
            ALWAYS,
            ONLY_SELECTED,
            NEVER
        }

        public delegate void ConfigChangeListener ();

        [SerializeField]
        private int innerPadding = 0;
        [SerializeField]
        private int valueIndicatorFontSize = 14;
        [SerializeField]
        private int valueIndicatorLineLength = 30;
        [SerializeField]
        private Color valueIndicatorColor = Color.white;
        [SerializeField]
        private ValueIndicatorVisibilityMode valueIndicatorVisibility = ValueIndicatorVisibilityMode.ONLY_SELECTED;
        [SerializeField]
        private ChartLabel valueLabelPrefab;

        public ConfigChangeListener configChangeListener;

        public int InnerPadding {
            get { return innerPadding; }
            set {
                innerPadding = value;
                OnConfigChanged ();
            }
        }

        public int ValueIndicatorFontSize {
            get { return valueIndicatorFontSize; }
            set {
                valueIndicatorFontSize = value;
                OnConfigChanged ();
            }
        }

        public int ValueIndicatorLineLength {
            get { return valueIndicatorLineLength; }
            set {
                valueIndicatorLineLength = value;
                OnConfigChanged ();
            }
        }

        public Color ValueIndicatorColor {
            get { return valueIndicatorColor; }
            set {
                valueIndicatorColor = value;
                OnConfigChanged ();
            }
        }

        public ValueIndicatorVisibilityMode ValueIndicatorVisibility {
            get { return valueIndicatorVisibility; }
            set {
                valueIndicatorVisibility = value;
                OnConfigChanged ();
            }
        }

        public ChartLabel ValueLabelPrefab {
            get { return valueLabelPrefab; }
            set {
                valueLabelPrefab = value;
                OnConfigChanged ();
            }
        }

        private void OnConfigChanged () {
            if (configChangeListener != null) {
                configChangeListener.Invoke ();
            }
        }
    }
}