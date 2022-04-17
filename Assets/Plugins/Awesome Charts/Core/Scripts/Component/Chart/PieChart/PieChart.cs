using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AwesomeCharts {

    [ExecuteInEditMode]
    public class PieChart : BaseChart<PieData>, IPointerClickHandler {

        public delegate void EntryClickDelegate (int index, PieEntry entry);

        [SerializeField]
        private PieData data;
        [SerializeField]
        private PieChartConfig config;

        public PieChartConfig Config {
            get { return config; }
            set {
                config = value;
                config.configChangeListener = OnConfigChanged;
                SetDirty ();
            }
        }

        private const float ENTRY_INDICATOR_LINE_SPACING = 5f;
        private const float SELECTED_ENTRY_OFFSET = 8f;

        private GameObject maskContainer = null;
        private List<PieChartEntryView> entryViews = new List<PieChartEntryView> ();
        private List<PieChartValueIndicator> valueIndicatorViews = new List<PieChartValueIndicator> ();
        private List<PieEntry> selectedEntries = new List<PieEntry> ();
        private List<EntryClickDelegate> clickDelegates = new List<EntryClickDelegate> ();

        public override PieData GetChartData () {
            return data;
        }

        void Reset () {
            data = new PieData (new PieDataSet ());
            config = new PieChartConfig ();
        }

        protected override void Awake () {
            base.Awake ();

            if (data == null) {
                data = new PieData (new PieDataSet ());
            }
            if (config == null) {
                config = new PieChartConfig ();
            }
        }

        override protected void Start () {
            entryViews = new List<PieChartEntryView> ();
            valueIndicatorViews = new List<PieChartValueIndicator> ();
            base.Start ();
        }

        private void OnConfigChanged () {
            SetDirty ();
        }

        protected override List<LegendEntry> CreateLegendViewEntries () {
            List<LegendEntry> result = new List<LegendEntry> ();

            if (data == null || data.DataSet == null)
                return result;

            foreach (PieEntry entry in data.DataSet.Entries) {
                result.Add (new LegendEntry (entry.Label,
                    entry.Color));
            }

            return result;
        }

        protected override void OnDrawChartContent () {
            base.OnDrawChartContent ();

            data.DataSet.RecalculateValues ();
            UpdateMaskContainer ();
            UpdateEntryViewsInstances (data.DataSet.GetEntriesCount ());
            UpdateValueIndicatorInstances (data.DataSet.GetEntriesCount ());
            FillEntryViews ();
            UpdateValueIndicators ();
        }

        private void UpdateMaskContainer () {
            if (maskContainer == null) {
                maskContainer = viewCreator.InstantiateMaskablePieChartObject ("mask",
                    chartDataContainerView.transform,
                    PivotValue.CENTER);
            }

            float size = Mathf.Max (Config.InnerPadding * 2, 0f);
            maskContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (size, size);
        }

        private void UpdateEntryViewsInstances (int requiredCount) {
            int currentCount = entryViews.Count;

            int missingCount = requiredCount - currentCount;
            while (missingCount > 0) {
                entryViews.Add (CreateEntryView ());
                missingCount--;
            }

            int redundantCount = currentCount - requiredCount;
            while (redundantCount > 0) {
                PieChartEntryView target = entryViews[entryViews.Count - 1];
                DestroyDelayed (target.gameObject);
                entryViews.Remove (target);
                redundantCount--;
            }
        }

        private PieChartEntryView CreateEntryView () {
            return viewCreator.InstantiatePieChartEntryView ("Pie entry",
                maskContainer.transform,
                PivotValue.CENTER);
        }

        private void UpdateValueIndicatorInstances (int requiredCount) {
            int currentCount = valueIndicatorViews.Count;

            int missingCount = requiredCount - currentCount;
            while (missingCount > 0) {
                valueIndicatorViews.Add (CreateValueIndicatorView ());
                missingCount--;
            }

            int redundantCount = currentCount - requiredCount;
            while (redundantCount > 0) {
                PieChartValueIndicator target = valueIndicatorViews[valueIndicatorViews.Count - 1];
                DestroyDelayed (target.gameObject);
                valueIndicatorViews.Remove (target);
                redundantCount--;
            }
        }

        private PieChartValueIndicator CreateValueIndicatorView () {
            return viewCreator.InstantiatePieEntryValueIndicator (config.ValueLabelPrefab,
                "Pie entry value indicator",
                contentView.transform,
                PivotValue.CENTER);
        }

        private void FillEntryViews () {
            PieDataSet dataSet = data.DataSet;
            for (int i = 0; i < dataSet.GetEntriesCount (); i++) {
                FillEntryView (entryViews[i],
                    dataSet.Entries[i],
                    dataSet.GetTotalValue (),
                    dataSet.GetPercentValue (i),
                    dataSet.GetRotationValue (i));
            }
        }

        private void FillEntryView (PieChartEntryView view,
            PieEntry entry,
            float totalValue,
            float percentValue,
            float rotation) {
            view.TotalValue = totalValue;
            view.Entry = entry;
            view.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -rotation));

            float centerAngle = rotation + (percentValue / 2 * 360);
            Vector2 position = selectedEntries.Contains (entry) ?
                MathUtils.GetPositionOnCircle (centerAngle, SELECTED_ENTRY_OFFSET) :
                Vector2.zero;

            float diameter = GetChartRadius () * 2;
            view.GetComponent<RectTransform> ().sizeDelta = new Vector2 (diameter, diameter);
            view.GetComponent<RectTransform> ().anchoredPosition = position;
        }

        private float GetChartRadius () {
            Vector2 chartSize = GetSize ();
            return Mathf.Min (chartSize.x / 2, chartSize.y / 2);
        }

        private void UpdateValueIndicators () {
            PieDataSet dataSet = data.DataSet;
            for (int i = 0; i < dataSet.GetEntriesCount (); i++) {
                if (ShouldValueIndicatorBeEnabled (dataSet.Entries[i])) {
                    valueIndicatorViews[i].gameObject.SetActive (true);
                    UpdateValueIndicatorView (valueIndicatorViews[i],
                        dataSet.Entries[i],
                        dataSet.GetRotationValue (i),
                        dataSet.GetPercentValue (i));
                } else {
                    valueIndicatorViews[i].gameObject.SetActive (false);
                }
            }
        }

        private bool ShouldValueIndicatorBeEnabled (PieEntry entry) {
            return Config.ValueIndicatorVisibility == PieChartConfig.ValueIndicatorVisibilityMode.ALWAYS ||
                (Config.ValueIndicatorVisibility == PieChartConfig.ValueIndicatorVisibilityMode.ONLY_SELECTED && selectedEntries.Contains (entry));
        }

        private void UpdateValueIndicatorView (PieChartValueIndicator view,
            PieEntry entry,
            float rotation,
            float percentValue) {

            float entryCenterAngle = rotation + (percentValue / 2 * 360);
            bool leftSide = entryCenterAngle % 360f > 180f;
            int leftSideMultiplier = leftSide ? -1 : 1;
            Vector2 position0 = MathUtils.GetPositionOnCircle (entryCenterAngle,
                GetChartRadius () + ENTRY_INDICATOR_LINE_SPACING +
                (selectedEntries.Contains (entry) ? SELECTED_ENTRY_OFFSET : 0f));
            Vector2 position1 = MathUtils.GetPositionOnCircle (entryCenterAngle,
                GetChartRadius () + Config.ValueIndicatorLineLength + ENTRY_INDICATOR_LINE_SPACING +
                (selectedEntries.Contains (entry) ? SELECTED_ENTRY_OFFSET : 0f));
            Vector2 position2 = position1 + new Vector2 (Config.ValueIndicatorLineLength * leftSideMultiplier, 0f);
            Vector2 positionLabel = position2 + new Vector2 (ENTRY_INDICATOR_LINE_SPACING * leftSideMultiplier, 0f);
            var linePoints = new List<Vector2> {
                position0 - positionLabel,
                position1 - positionLabel,
                position2 - positionLabel
            };

            view.GetComponent<RectTransform> ().anchoredPosition = positionLabel;
            view.Label = CreateValueIndicatorLabel (entry, percentValue);
            view.FontSize = Config.ValueIndicatorFontSize;
            view.IndicatorColor = Config.ValueIndicatorColor;
            view.LinePoints = linePoints;
            view.ReversedLabel = leftSide;
        }

        private String CreateValueIndicatorLabel (PieEntry entry, float percentValue) {
            return String.Format ("{0}: {1}%", entry.Label, (percentValue * 100).ToString ("0.00"));
        }

        public void OnPointerClick (PointerEventData eventData) {
            Vector2 localPoint;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle (
                    GetComponent<RectTransform> (),
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPoint
                )) {
                return;
            }

            float distance = Vector2.Distance (localPoint, Vector2.zero);
            if (distance <= GetChartRadius () && distance > Config.InnerPadding) {
                double angle = MathUtils.GetAngle (Vector2.zero, localPoint);
                angle = MathUtils.AngleToCircleAngle (angle);
                int entryIndex = data.DataSet.EntryIndexForAngle (angle);
                if (entryIndex > -1) {
                    OnEntryClick (entryIndex, data.DataSet.Entries[entryIndex]);
                }
            }
        }

        private void OnEntryClick (int index, PieEntry entry) {
            for (int i = 0; i < clickDelegates.Count; i++) {
                clickDelegates[i].Invoke (index, entry);
            }
        }

        public bool IsEntryAtPositionSelected (int position) {
            return selectedEntries.Contains (data.DataSet.GetEntryAt (position));
        }

        public void SelectEntryAtPosition (int position) {
            PieEntry entry = data.DataSet.GetEntryAt (position);
            if (entry != null && !selectedEntries.Contains (entry)) {
                selectedEntries.Add (entry);
                SetDirty ();
            }
        }

        public void DeselectEntryAtPosition (int position) {
            PieEntry entry = data.DataSet.GetEntryAt (position);
            if (entry != null && selectedEntries.Contains (entry)) {
                selectedEntries.Remove (entry);
                SetDirty ();
            }
        }

        public void AddEntryClickDelegate (EntryClickDelegate clickDelegate) {
            if (!clickDelegates.Contains (clickDelegate)) {
                clickDelegates.Add (clickDelegate);
            }
        }

        public void RemoveEntryClickDelegate (EntryClickDelegate clickDelegate) {
            if (clickDelegates.Contains (clickDelegate)) {
                clickDelegates.Remove (clickDelegate);
            }
        }
    }
}