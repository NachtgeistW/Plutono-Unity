using System;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {

    public abstract class AxisBaseChart<T> : BaseChart<T> where T : AxisChartData {

        protected abstract AxisLabelEntryProvider GetVerticalAxisEntriesProvider ();

        protected abstract AxisLabelEntryProvider GetHorizontalAxisEntriesProvider ();

        protected abstract SingleAxisConfig GetVerticalAxisConfig ();

        protected abstract SingleAxisConfig GetHorizontalAxisConfig ();

        [SerializeField]
        private GridConfig gridConfig;
        [SerializeField]
        private GridFrameConfig frameConfig;

        protected GridRenderer gridRenderer;
        protected FrameRenderer frameRenderer;
        protected AxisLabelRenderer verticalAxisLabelRenderer;
        protected AxisLabelRenderer horizontalAxisLabelRenderer;

        public GridConfig GridConfig {
            get { return gridConfig; }
            set {
                gridConfig = value;
                SetDirty ();
            }
        }

        public GridFrameConfig FrameConfig {
            get { return frameConfig; }
            set {
                frameConfig = value;
                SetDirty ();
            }
        }

        protected virtual void OnUpdateAxis () {
            if(verticalAxisLabelRenderer == null)
                verticalAxisLabelRenderer = InstantiateVerticalAxisLabelRenderer ();
            if(horizontalAxisLabelRenderer == null)
                horizontalAxisLabelRenderer = InstantiateHorizontalAxisLabelRenderer ();
         }

        protected override void Awake () {
            base.Awake ();

            if (gridConfig == null)
                gridConfig = new GridConfig ();
            if (frameConfig == null)
                frameConfig = new GridFrameConfig ();
        }

        protected override void OnInstantiateViews () {
            base.OnInstantiateViews ();

            gridRenderer = InstantiateGridRenderer ();
            gridRenderer.transform.SetSiblingIndex (0);
            frameRenderer = InstantiateFrameRenderer ();
            frameRenderer.raycastTarget = false;
            frameRenderer.transform.SetSiblingIndex (chartDataContainerView.transform.GetSiblingIndex () + 1);
            verticalAxisLabelRenderer = InstantiateVerticalAxisLabelRenderer ();
            horizontalAxisLabelRenderer = InstantiateHorizontalAxisLabelRenderer ();
        }

        private GridRenderer InstantiateGridRenderer () {
            return viewCreator.InstantiateGridRenderer ("GridRenderer", contentView.transform, PivotValue.BOTTOM_LEFT);
        }

        private FrameRenderer InstantiateFrameRenderer () {
            return viewCreator.InstantiateFrameRenderer ("FrameRenderer", contentView.transform, PivotValue.BOTTOM_LEFT);
        }

        private AxisLabelRenderer InstantiateVerticalAxisLabelRenderer () {
            return viewCreator.InstantiateAxisLabelRenderer ("VerticalAxisLabelRenderer", contentView.transform, PivotValue.BOTTOM_LEFT);
        }

        private AxisLabelRenderer InstantiateHorizontalAxisLabelRenderer () {
            return viewCreator.InstantiateAxisLabelRenderer ("HorizontalAxisLabelRenderer", contentView.transform, PivotValue.BOTTOM_LEFT);
        }

        protected override void OnUpdateViewsSize (Vector2 size) {
            base.OnUpdateViewsSize (size);

            gridRenderer.GetComponent<RectTransform> ().sizeDelta = size;
            frameRenderer.GetComponent<RectTransform> ().sizeDelta = size;
            verticalAxisLabelRenderer.GetComponent<RectTransform> ().sizeDelta = size;
            horizontalAxisLabelRenderer.GetComponent<RectTransform> ().sizeDelta = size;
        }

        protected override void OnDrawChartContent () {
            base.OnDrawChartContent ();
            UpdateAxis ();
        }

        private void UpdateAxis () {
            if (GetChartData () == null)
                return;

            OnUpdateAxis ();

            gridRenderer.GridConfig = GridConfig;
            frameRenderer.GridFrameConfig = FrameConfig;

            verticalAxisLabelRenderer.ObjectPrefab = GetVerticalAxisConfig ().AxisLabelPrefab;
            verticalAxisLabelRenderer.Entries = GetVerticalAxisEntriesProvider ().getLabelRendererEntries ();
            verticalAxisLabelRenderer.LabelsConfig = GetVerticalAxisConfig ().LabelsConfig;
            verticalAxisLabelRenderer.Reload ();

            horizontalAxisLabelRenderer.ObjectPrefab = GetHorizontalAxisConfig ().AxisLabelPrefab;
            horizontalAxisLabelRenderer.Entries = GetHorizontalAxisEntriesProvider ().getLabelRendererEntries ();
            horizontalAxisLabelRenderer.LabelsConfig = GetHorizontalAxisConfig ().LabelsConfig;
            horizontalAxisLabelRenderer.Reload ();
        }

        protected AxisBounds GetAxisBounds () {
            AxisValue verticalAxisBounds = GetVerticalAxisConfig ().Bounds;
            AxisValue horizontalAxisBounds = GetHorizontalAxisConfig ().Bounds;

            float xMin = horizontalAxisBounds.MinAutoValue ? GetChartData ().GetMinPosition () : horizontalAxisBounds.Min;
            float xMax = horizontalAxisBounds.MaxAutoValue ? GetChartData ().GetMaxPosition () : horizontalAxisBounds.Max;
            float yMin = verticalAxisBounds.MinAutoValue ? GetClosestRoundValue (GetChartData ().GetMinValue (), GetChartData ().GetMinValue () < 0) :
                verticalAxisBounds.Min;
            float yMax = verticalAxisBounds.MaxAutoValue ? GetClosestRoundValue (GetChartData ().GetMaxValue (), GetChartData ().GetMaxValue () > 0) :
                verticalAxisBounds.Max;

            return new AxisBounds (xMin, xMax, yMin, yMax);
        }

        private int GetClosestRoundValue (float value, bool up) {
            if (value == 0)
                return 0;

            float valueRoundDifference = CalculateRoundingDifferenceForValue (value);
            if (up) {
                return (int) (value + valueRoundDifference);
            } else {
                return (int) (value - valueRoundDifference);
            }
        }

        private float CalculateRoundingDifferenceForValue (float value) {
            int signMultiplyer = value >= 0 ? 1 : -1;
            float currentValue = Math.Abs (value * 1.1f);
            float log10 = Mathf.FloorToInt (Mathf.Log10 (currentValue) + 1);
            currentValue = Mathf.Ceil (currentValue);
            if (log10 > 2) {
                currentValue = ((int) (currentValue / Mathf.Pow (10, log10 - 2)) + 1) * Mathf.Pow (10, log10 - 2);
            } else if (log10 >= 1) {
                currentValue = ((int) (currentValue / Mathf.Pow (10, log10 - 1)) + 1) * Mathf.Pow (10, log10 - 1);
            }
            return (currentValue - Math.Abs (value)) * signMultiplyer;
        }
    }
}