using UnityEngine;
using UnityEngine.Events;
using System;

namespace AwesomeCharts {

    public delegate void LineChartAction(LineEntry entry, int dataSetIndex);

    [Serializable]
    public class LineChartConfig {

        public delegate void ConfigChangeListener();

        [SerializeField]
        private int valueIndicatorSize = Defaults.LINE_CHART_VALUE_INDICATOR_SIZE;

        [SerializeField]
        private bool showValueIndicators = true;

        [SerializeField]
        private ChartValuePopup popupPrefab;

        [SerializeField]
        private LineChartAction onValueClickAction;

        public ConfigChangeListener configChangeListener;

        public int ValueIndicatorSize {
            get { return valueIndicatorSize; }
            set {
                valueIndicatorSize = value;
                OnConfigChanged();
            }
        }

        public bool ShowValueIndicators {
            get { return showValueIndicators; }
            set {
                showValueIndicators = value;
                OnConfigChanged();
            }
        }

        public ChartValuePopup PopupPrefab {
            get { return popupPrefab; }
            set {
                popupPrefab = value;
                OnConfigChanged();
            }
        }

        public LineChartAction OnValueClickAction {
            get { return onValueClickAction; }
            set {
                onValueClickAction = value;
                OnConfigChanged();
            }
        }

        private void OnConfigChanged() {
            if(configChangeListener != null) {
                configChangeListener.Invoke();
            }
        }
    }
}
