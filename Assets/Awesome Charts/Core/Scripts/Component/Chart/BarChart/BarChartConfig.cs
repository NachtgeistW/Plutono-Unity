using UnityEngine;
using System;

namespace AwesomeCharts {

    public delegate void BarChartAction(BarEntry entry, int dataSetIndex);

    [Serializable]
    public class BarChartConfig {

        public delegate void ConfigChangeListener();

        [SerializeField]
        private int barWidth = Defaults.BAR_WIDTH;

        [SerializeField]
        private int barSpacing = Defaults.BAR_SPACING;

        [SerializeField]
        private int innerBarSpacing = Defaults.INNER_BAR_SPACING;

        [SerializeField]
        private BarSizingMethod sizingMethod = Defaults.BAR_SIZING_METHOD;

        [SerializeField]
        private Bar barPrefab;

        [SerializeField]
        private ChartValuePopup popupPrefab;

        [SerializeField]
        private BarChartAction barChartClickAction;

        internal ConfigChangeListener configChangeListener;

        public int BarWidth {
            get { return barWidth; }
            set {
                barWidth = value;
                OnConfigChanged();
            }
        }

        public int BarSpacing {
            get { return barSpacing; }
            set {
                barSpacing = value;
                OnConfigChanged();
            }
        }

        public int InnerBarSpacing {
            get { return innerBarSpacing; }
            set {
                innerBarSpacing = value;
                OnConfigChanged();
            }
        }

        public BarSizingMethod SizingMethod {
            get { return sizingMethod; }
            set {
                sizingMethod = value;
                OnConfigChanged();
            }
        }

        public Bar BarPrefab {
            get { return barPrefab; }
            set {
                barPrefab = value;
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

        public BarChartAction BarChartClickAction {
            get { return barChartClickAction; }
            set {
                barChartClickAction = value;
                OnConfigChanged();
            }
        }

        private void OnConfigChanged() {
            if(configChangeListener != null){
                configChangeListener();
            }
        }
    }
}
