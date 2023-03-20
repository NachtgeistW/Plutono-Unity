using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    [ExecuteInEditMode]
    public class BarChart : AxisBaseChart<BarData> {

        [SerializeField]
        private BarChartAxisConfig axisConfig;
        [SerializeField]
        private BarChartConfig config;
        [SerializeField]
        internal BarData data;

        private BarCharPositioner positioner;
        private ChartValuePopup currentValuePopup = null;
        private BarEntry currentValuePopupEntry = null;
        private VerticalAxisLabelEntryProvider verticalLabelsProvider;
        private BarAxisLabelEntryProvider horizontalLabelsProvider;
        private BasicAxisValueFormatter verticalAxisValueFormatter;
        private BarAxisValueFormatter horizontalAxisValueFormatter;
        private AxisValueFormatter customVerticalAxisValueFormatter;
        private AxisValueFormatter customHorizontalAxisValueFormatter;

        private List<Bar> barInstances;

        public BarChartConfig Config {
            get { return config; }
            set {
                config = value;
                config.configChangeListener = OnConfigChanged;
                SetDirty ();
            }
        }

        public BarChartAxisConfig AxisConfig {
            get { return axisConfig; }
            set {
                axisConfig = value;
                SetDirty ();
            }
        }

        public AxisValueFormatter CustomVerticalAxisValueFormatter {
            get { return customVerticalAxisValueFormatter; }
            set {
                customVerticalAxisValueFormatter = value;
                SetDirty ();
            }
        }

        public AxisValueFormatter CustomHorizontalAxisValueFormatter {
            get { return customHorizontalAxisValueFormatter; }
            set {
                customHorizontalAxisValueFormatter = value;
                SetDirty ();
            }
        }

        public override BarData GetChartData () {
            return data;
        }

        void Reset () {
            data = new BarData (new BarDataSet ());
            Config = new BarChartConfig ();
        }

        protected override void Awake () {
            base.Awake ();

            if (data == null) {
                data = new BarData (new BarDataSet ());
            }
            if (config == null) {
                Config = new BarChartConfig ();
            }
            if (axisConfig == null) {
                axisConfig = new BarChartAxisConfig ();
            }

            positioner = new BarCharPositioner ();
            barInstances = new List<Bar> ();
            verticalLabelsProvider = new VerticalAxisLabelEntryProvider ();
            horizontalLabelsProvider = new BarAxisLabelEntryProvider ();
            verticalAxisValueFormatter = new BasicAxisValueFormatter ();
            horizontalAxisValueFormatter = new BarAxisValueFormatter ();
        }

        private void OnConfigChanged () {
            SetDirty ();
        }

        protected override void OnInstantiateViews () {
            base.OnInstantiateViews ();
            chartDataContainerView.AddComponent<Image> ();
            var mask = chartDataContainerView.AddComponent<Mask> ();
            mask.showMaskGraphic = false;
        }

        protected override AxisLabelEntryProvider GetVerticalAxisEntriesProvider () {
            return verticalLabelsProvider;
        }

        protected override AxisLabelEntryProvider GetHorizontalAxisEntriesProvider () {
            return horizontalLabelsProvider;
        }

        protected override SingleAxisConfig GetVerticalAxisConfig () {
            return axisConfig.VerticalAxisConfig;
        }

        protected override SingleAxisConfig GetHorizontalAxisConfig () {
            return axisConfig.HorizontalAxisConfig;
        }

        private AxisValueFormatter GetCorrectVerticalAxisValueFormatter () {
            return CustomVerticalAxisValueFormatter != null? CustomVerticalAxisValueFormatter : verticalAxisValueFormatter;
        }

        private AxisValueFormatter GetCorrectHorizontalAxisValueFormatter () {
            return CustomHorizontalAxisValueFormatter != null? CustomHorizontalAxisValueFormatter : horizontalAxisValueFormatter;
        }

        protected override void OnUpdateAxis () {
            base.OnUpdateAxis ();
            UpdateBarChartPositioner ();
            UpdateHorizontalAxisEntriesProvider ();
            UpdateVerticalAxisEntriesProvider ();
        }

        private void UpdateBarChartPositioner () {
            positioner.data = data;
            positioner.barChartConfig = Config;
            positioner.containerSize = GetSize ();
            positioner.axisBounds = GetAxisBounds ();
            positioner.RecalculatePositioner ();
        }

        private void UpdateVerticalAxisEntriesProvider () {
            verticalAxisValueFormatter.config = AxisConfig.VerticalAxisConfig.ValueFormatterConfig;

            AxisBounds axisBounds = GetAxisBounds ();
            verticalLabelsProvider.valueMin = axisBounds.YMin;
            verticalLabelsProvider.valueMax = axisBounds.YMax;
            verticalLabelsProvider.labelCount = AxisConfig.VerticalAxisConfig.LabelsCount;
            verticalLabelsProvider.firstEntryVisible = AxisConfig.VerticalAxisConfig.DrawStartValue;
            verticalLabelsProvider.lastEntryVisible = AxisConfig.VerticalAxisConfig.DrawEndValue;
            verticalLabelsProvider.labelsGravity = AxisConfig.VerticalAxisConfig.LabelsAlignment;
            verticalLabelsProvider.valueFormatter = GetCorrectVerticalAxisValueFormatter ();
            verticalLabelsProvider.axisLength = verticalAxisLabelRenderer.GetComponent<RectTransform> ().sizeDelta.y;
        }

        private void UpdateHorizontalAxisEntriesProvider () {
            horizontalAxisValueFormatter.config = AxisConfig.HorizontalAxisConfig.ValueFormatterConfig;

            horizontalLabelsProvider.barChartPositioner = positioner;
            horizontalLabelsProvider.valueFormatter = GetCorrectHorizontalAxisValueFormatter ();
            horizontalLabelsProvider.labelsGravity = AxisConfig.HorizontalAxisConfig.LabelsAlignment;
        }

        protected override List<LegendEntry> CreateLegendViewEntries () {
            List<LegendEntry> result = new List<LegendEntry> ();
            foreach (BarDataSet dataSet in data.DataSets) {
                result.Add (new LegendEntry (dataSet.Title,
                    dataSet.GetColorForIndex (0)));
            }

            return result;
        }

        protected override void OnDrawChartContent () {
            base.OnDrawChartContent ();

            HideCurrentValuePopup ();
            UpdateBarInstances (positioner.GetAllVisibleEntriesCount ());
            if (GetChartData ().HasAnyData ()) {
                ShowBars ();
            }
        }

        private void UpdateBarInstances (int requiredCount) {
            int currentBarsCount = barInstances.Count;

            // Add missing bars
            int missingBarsCount = requiredCount - currentBarsCount;
            while (missingBarsCount > 0) {
                Bar barInstance = viewCreator.InstantiateBar (chartDataContainerView.transform, Config.BarPrefab);
                barInstances.Add (barInstance);
                missingBarsCount--;
            }

            // Remove redundant bars
            int redundantBarsCount = currentBarsCount - requiredCount;
            while (redundantBarsCount > 0) {
                Bar target = barInstances[barInstances.Count - 1];
                DestroyDelayed (target.gameObject);
                barInstances.Remove (target);
                redundantBarsCount--;
            }
        }

        private void ShowBars () {
            int nextBarInstanceIndex = 0;

            for (int i = 0; i < data.DataSets.Count; i++) {
                nextBarInstanceIndex = UpdateBars (i, nextBarInstanceIndex);
            }
        }

        private int UpdateBars (int dataSetIndex, int nextBarInstanceIndex) {
            List<BarEntry> barEntries = positioner.GetVisibleEntries (dataSetIndex);
            for (int i = 0; i < barEntries.Count; i++) {
                UpdateBarWithEntry (barInstances[nextBarInstanceIndex],
                    barEntries[i],
                    data.DataSets[dataSetIndex].GetColorForIndex (i),
                    dataSetIndex);
                nextBarInstanceIndex++;
            }

            return nextBarInstanceIndex;
        }

        private Bar UpdateBarWithEntry (Bar barInstance, BarEntry entry, Color color, int dataSetIndex) {
            barInstance.transform.localPosition = positioner.GetBarPosition ((int) entry.Position, dataSetIndex);
            barInstance.GetComponent<RectTransform> ().sizeDelta = positioner.GetBarSize (entry.Value);
            barInstance.SetColor (color);
            barInstance.button.onClick.RemoveAllListeners ();
            barInstance.button.onClick.AddListener (delegate { OnBarClick (entry, dataSetIndex); });

            return barInstance;
        }

        private void OnBarClick (BarEntry entry, int dataSetIndex) {
            if (Config.BarChartClickAction != null) {
                Config.BarChartClickAction.Invoke (entry, dataSetIndex);
            }
            ShowHideValuePopup (entry, dataSetIndex);
        }

        private void ShowHideValuePopup (BarEntry entry, int dataSetIndex) {
            if (currentValuePopup == null) {
                currentValuePopup = viewCreator.InstantiateChartPopup (contentView.transform, Config.PopupPrefab);
            }

            if (entry != currentValuePopupEntry) {
                UpdateValuePopup (entry, dataSetIndex);
                currentValuePopupEntry = entry;
            } else {
                HideCurrentValuePopup ();
            }
        }

        private void HideCurrentValuePopup () {
            if (currentValuePopup != null) {
                currentValuePopup.gameObject.SetActive (false);
                currentValuePopupEntry = null;
            }
        }

        private void UpdateValuePopup (BarEntry entry, int dataSetIndex) {
            currentValuePopup.transform.localPosition = positioner.GetValuePopupPosition (entry, dataSetIndex);
            currentValuePopup.text.SetLabelText("" + entry.Value);
            currentValuePopup.gameObject.SetActive (true);
        }
    }
}