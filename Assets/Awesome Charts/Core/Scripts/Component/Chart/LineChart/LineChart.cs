using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace AwesomeCharts {

    [ExecuteInEditMode]
    public class LineChart : AxisBaseChart<LineData> {

        private const int BEZIER_LINE_SEGMENTS = 10;

        [SerializeField]
        private LineChartAxisConfig axisConfig;
        [SerializeField]
        private LineChartConfig config;
        [SerializeField]
        private LineData data;

        private ChartValuePopup currentValuePopup = null;
        private LineEntry currentValuePopupEntry = null;
        private Vector2[][] entriesPoints;
        private Vector2[][] bezierPoints;
        private List<LineEntryIdicator> entryIdicators = new List<LineEntryIdicator> ();
        private List<UILineRenderer> lineRenderers = new List<UILineRenderer> ();
        private List<LineChartBackground> lineBackgrounds = new List<LineChartBackground> ();
        private VerticalAxisLabelEntryProvider verticalLabelsProvider;
        private HorizontalAxisLabelEntryProvider horizontalLabelsProvider;
        private BasicAxisValueFormatter verticalAxisValueFormatter;
        private BasicAxisValueFormatter horizontalAxisValueFormatter;
        private AxisValueFormatter customVerticalAxisValueFormatter;
        private AxisValueFormatter customHorizontalAxisValueFormatter;

        public override LineData GetChartData () {
            return data;
        }

        public LineChartConfig Config {
            get { return config; }
            set {
                config = value;
                config.configChangeListener = OnConfigChanged;
                SetDirty ();
            }
        }

        public LineChartAxisConfig AxisConfig {
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

        void Reset () {
            data = new LineData (new LineDataSet ());
            config = new LineChartConfig ();
        }

        protected override void Awake () {
            base.Awake ();

            if (data == null) {
                data = new LineData (new LineDataSet ());
            }
            if (config == null) {
                config = new LineChartConfig ();
            }
            if (axisConfig == null) {
                axisConfig = new LineChartAxisConfig ();
            }

            verticalLabelsProvider = new VerticalAxisLabelEntryProvider ();
            horizontalLabelsProvider = new HorizontalAxisLabelEntryProvider ();
            verticalAxisValueFormatter = new BasicAxisValueFormatter ();
            horizontalAxisValueFormatter = new BasicAxisValueFormatter ();
        }

        private void OnConfigChanged () {
            SetDirty ();
        }

        protected override List<LegendEntry> CreateLegendViewEntries () {
            List<LegendEntry> result = new List<LegendEntry> ();
            foreach (LineDataSet dataSet in data.DataSets) {
                result.Add (new LegendEntry (dataSet.Title,
                    dataSet.LineColor));
            }

            return result;
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
            return AxisConfig.VerticalAxisConfig;
        }

        protected override SingleAxisConfig GetHorizontalAxisConfig () {
            return AxisConfig.HorizontalAxisConfig;
        }

        private AxisValueFormatter GetCorrectVerticalAxisValueFormatter () {
            return CustomVerticalAxisValueFormatter != null? CustomVerticalAxisValueFormatter : verticalAxisValueFormatter;
        }

        private AxisValueFormatter GetCorrectHorizontalAxisValueFormatter () {
            return CustomHorizontalAxisValueFormatter != null? CustomHorizontalAxisValueFormatter : horizontalAxisValueFormatter;
        }

        protected override void OnUpdateAxis () {
            AxisBounds axisBounds = GetAxisBounds ();

            horizontalAxisValueFormatter.config = AxisConfig.HorizontalAxisConfig.ValueFormatterConfig;
            verticalAxisValueFormatter.config = AxisConfig.VerticalAxisConfig.ValueFormatterConfig;

            horizontalLabelsProvider.valueMin = axisBounds.XMin;
            horizontalLabelsProvider.valueMax = axisBounds.XMax;
            horizontalLabelsProvider.labelCount = AxisConfig.HorizontalAxisConfig.LabelsCount;
            horizontalLabelsProvider.firstEntryVisible = AxisConfig.HorizontalAxisConfig.DrawStartValue;
            horizontalLabelsProvider.lastEntryVisible = AxisConfig.HorizontalAxisConfig.DrawEndValue;
            horizontalLabelsProvider.labelsGravity = AxisConfig.HorizontalAxisConfig.LabelsAlignment;
            horizontalLabelsProvider.valueFormatter = GetCorrectHorizontalAxisValueFormatter ();
            horizontalLabelsProvider.axisLength = horizontalAxisLabelRenderer.GetComponent<RectTransform> ().sizeDelta.x;

            verticalLabelsProvider.valueMin = axisBounds.YMin;
            verticalLabelsProvider.valueMax = axisBounds.YMax;
            verticalLabelsProvider.labelCount = AxisConfig.VerticalAxisConfig.LabelsCount;
            verticalLabelsProvider.firstEntryVisible = AxisConfig.VerticalAxisConfig.DrawStartValue;
            verticalLabelsProvider.lastEntryVisible = AxisConfig.VerticalAxisConfig.DrawEndValue;
            verticalLabelsProvider.labelsGravity = AxisConfig.VerticalAxisConfig.LabelsAlignment;
            verticalLabelsProvider.valueFormatter = GetCorrectVerticalAxisValueFormatter ();
            verticalLabelsProvider.axisLength = verticalAxisLabelRenderer.GetComponent<RectTransform> ().sizeDelta.y;
        }

        protected override void OnUpdateViewsSize (Vector2 size) {
            base.OnUpdateViewsSize (size);

            lineRenderers.ForEach (renderer => {
                renderer.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            });

            lineBackgrounds.ForEach (renderer => {
                renderer.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            });
        }

        protected override void OnDrawChartContent () {
            base.OnDrawChartContent ();

            HideCurrentValuePopup ();
            CalculateLinesPoints ();

            UpdateLineRendererInstances (GetChartData ().DataSets.Count);
            UpdateBackgroundInstances (GetChartData ().DataSets.Count);
            UpdateEntryIndicatorInstances (GetRequiredValueIndicatorsCount ());

            DrawLines ();
        }

        private int GetRequiredValueIndicatorsCount () {
            return Config.ShowValueIndicators? GetAllVisibleEntriesCount () : 0;
        }

        private int GetAllVisibleEntriesCount () {
            int result = 0;
            for (int i = 0; i < entriesPoints.Length; i++) {
                result += entriesPoints[i].Length;
            }

            return result;
        }

        private void UpdateLineRendererInstances (int requiredCount) {
            int currentCount = lineRenderers.Count;

            // Add missing renderers
            int missingCount = requiredCount - currentCount;
            while (missingCount > 0) {
                lineRenderers.Add (CreateLineRenderer ());
                missingCount--;
            }

            // Remove redundant renderers
            int redundantCount = currentCount - requiredCount;
            while (redundantCount > 0) {
                UILineRenderer target = lineRenderers[lineRenderers.Count - 1];
                DestroyImmediate (target.gameObject);
                lineRenderers.Remove (target);
                redundantCount--;
            }
        }

        private UILineRenderer CreateLineRenderer () {
            UILineRenderer lineRenderer = viewCreator.InstantiateLineRenderer ("Line",
                chartDataContainerView.transform, PivotValue.BOTTOM_LEFT);
            lineRenderer.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            return lineRenderer;
        }

        private void UpdateBackgroundInstances (int requiredCount) {
            int currentCount = lineBackgrounds.Count;

            // Add missing backgrounds
            int missingCount = requiredCount - currentCount;
            while (missingCount > 0) {
                LineChartBackground chartBackground = CreateLineBackground ();
                chartBackground.transform.SetSiblingIndex (0);
                lineBackgrounds.Add (chartBackground);
                missingCount--;
            }

            // Remove redundant backgrounds
            int redundantCount = currentCount - requiredCount;
            while (redundantCount > 0) {
                LineChartBackground target = lineBackgrounds[lineBackgrounds.Count - 1];
                DestroyImmediate (target.gameObject);
                lineBackgrounds.Remove (target);
                redundantCount--;
            }
        }

        private LineChartBackground CreateLineBackground () {
            LineChartBackground lineBackground = viewCreator.InstantiateLineBackground ("LineBackground",
                chartDataContainerView.transform, PivotValue.BOTTOM_LEFT);
            lineBackground.GetComponent<RectTransform> ().sizeDelta = GetSize ();
            return lineBackground;
        }

        private void UpdateEntryIndicatorInstances (int requiredCount) {
            int currentCount = entryIdicators.Count;

            // Add missing indicators
            int missingCount = requiredCount - currentCount;
            while (missingCount > 0) {
                LineEntryIdicator indicator = viewCreator.InstantiateCircleImage ("dot", contentView.transform);
                entryIdicators.Add (indicator);
                missingCount--;
            }

            // Remove redundant indicators
            int redundantCount = currentCount - requiredCount;
            while (redundantCount > 0) {
                LineEntryIdicator target = entryIdicators[entryIdicators.Count - 1];
                DestroyImmediate (target.gameObject);
                entryIdicators.Remove (target);
                redundantCount--;
            }
        }

        private void CalculateLinesPoints () {
            int dataSetsCount = GetChartData ().DataSets.Count;
            entriesPoints = new Vector2[dataSetsCount][];
            bezierPoints = new Vector2[dataSetsCount][];
            for (int i = 0; i < dataSetsCount; i++) {
                CalculateDataSetLinePoints (GetChartData ().DataSets[i], i);
            }
        }

        private void CalculateDataSetLinePoints (LineDataSet dataSet, int dataSetIndex) {
            entriesPoints[dataSetIndex] = CreateLinePointsFromDataSet (dataSet);
            if (dataSet.UseBezier && dataSet.GetEntriesCount () > 2) {
                bezierPoints[dataSetIndex] = BezierUtils.CreateBezierPointsFromLinePoints (entriesPoints[dataSetIndex]);
            } else {
                bezierPoints[dataSetIndex] = new Vector2[0];
            }
        }

        private Vector2[] CreateLinePointsFromDataSet (LineDataSet dataSet) {
            if (dataSet.GetEntriesCount () < 1 /*  || dataSet.PositionDelta () <= 0f */ ) {
                return new Vector2[0];
            }

            AxisBounds axisBounds = GetAxisBounds ();
            List<LineEntry> sortedEntries = dataSet.GetSortedEntries ();
            Vector2 chartSize = GetSize ();

            float positionDelta = axisBounds.XMax - axisBounds.XMin;
            float valueDelta = axisBounds.YMax - axisBounds.YMin;
            int index = 0;

            Vector2[] result = new Vector2[dataSet.Entries.Count];
            sortedEntries.ForEach (entry => {
                float pointX = positionDelta > 0f?((entry.Position - axisBounds.XMin) / positionDelta) * chartSize.x : 0f;
                float pointY = valueDelta > 0f?((entry.Value - axisBounds.YMin) / valueDelta) * chartSize.y : 0f;
                result[index] = new Vector2 (pointX, pointY);
                index++;
            });

            return result;
        }

        private void DrawLines () {
            int dataSetsCount = GetChartData ().DataSets.Count;
            int currentIndicatorPosition = 0;
            for (int i = 0; i < dataSetsCount; i++) {
                DrawLineBackground (lineBackgrounds[i], GetChartData ().DataSets[i], i);

                currentIndicatorPosition = DrawLine (lineRenderers[i],
                    GetChartData ().DataSets[i],
                    i,
                    currentIndicatorPosition);
            }
        }

        private void DrawLineBackground (LineChartBackground lineBackground, LineDataSet dataSet, int dataSetIndex) {
            lineBackground.color = dataSet.FillColor;
            lineBackground.Texture = dataSet.FillTexture;
            lineBackground.AxisBounds = GetAxisBounds ();

            lineBackground.Points = dataSet.UseBezier ?
                CalculateBezierSegmentsPoints (bezierPoints[dataSetIndex]) :
                entriesPoints[dataSetIndex];
        }

        private Vector2[] CalculateBezierSegmentsPoints (Vector2[] controlPoints) {
            BezierPath bezierPath = new BezierPath {
                SegmentsPerCurve = BEZIER_LINE_SEGMENTS
            };
            bezierPath.SetControlPoints (controlPoints);

            return bezierPath.GetDrawingPoints0 ().ToArray ();
        }

        private int DrawLine (UILineRenderer lineRenderer, LineDataSet dataSet, int dataSetIndex, int currentIndicatorPosition) {
            List<LineEntry> entries = dataSet.GetSortedEntries ();
            Color32 lineColor = dataSet.LineColor;

            lineRenderer.lineThickness = dataSet.LineThickness;
            lineRenderer.color = lineColor;
            lineRenderer.m_points = dataSet.UseBezier ? bezierPoints[dataSetIndex] :
                entriesPoints[dataSetIndex];
            lineRenderer.BezierSegmentsPerCurve = BEZIER_LINE_SEGMENTS;
            lineRenderer.BezierMode = dataSet.UseBezier ? UILineRenderer.BezierType.Basic :
                UILineRenderer.BezierType.None;
            lineRenderer.SetAllDirty ();

            if (Config.ShowValueIndicators) {
                return UpdateCirclesAtPosition (entriesPoints[dataSetIndex],
                    entries.ToArray (),
                    lineColor,
                    currentIndicatorPosition,
                    dataSetIndex);
            } else {
                return 0;
            }
        }

        private int UpdateCirclesAtPosition (Vector2[] positions, LineEntry[] entries, Color32 color, int firstAvailableIndicatorPosition, int dataSetIndex) {
            Vector2 contentSize = GetContentSize ();
            for (int i = 0; i < positions.Length; i++) {
                Vector2 position = positions[i];
                bool indicatorOutside = position.x < 0 || position.x > contentSize.x || position.y < 0 || position.y > contentSize.y;

                LineEntryIdicator indicator = entryIdicators[firstAvailableIndicatorPosition + i];
                indicator.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Config.ValueIndicatorSize, Config.ValueIndicatorSize);
                indicator.transform.localPosition = position;
                indicator.entry = entries[i];
                indicator.image.color = color;
                indicator.gameObject.SetActive (!indicatorOutside);
                indicator.button.onClick.RemoveAllListeners ();
                indicator.button.onClick.AddListener (delegate {
                    OnEntryClick (indicator, dataSetIndex);
                });
            }

            return firstAvailableIndicatorPosition + positions.Length;
        }

        private void OnEntryClick (LineEntryIdicator indicator, int dataSetIndex) {
            if (Config.OnValueClickAction != null) {
                Config.OnValueClickAction.Invoke (indicator.entry, dataSetIndex);
            }
            ShowHideValuePopup (indicator);
        }

        private void ShowHideValuePopup (LineEntryIdicator indicator) {
            if (currentValuePopup == null) {
                currentValuePopup = viewCreator.InstantiateChartPopup (contentView.transform, Config.PopupPrefab);
            }

            if (indicator.entry != currentValuePopupEntry) {
                UpdateValuePopup (indicator);
                currentValuePopupEntry = indicator.entry;
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

        private void UpdateValuePopup (LineEntryIdicator indicator) {
            currentValuePopup.transform.localPosition = PopupPositionFromEntry (indicator);
            currentValuePopup.text.SetLabelText("" + indicator.entry.Value);
            currentValuePopup.gameObject.SetActive (true);
        }

        private Vector3 PopupPositionFromEntry (LineEntryIdicator indicator) {
            return new Vector3 (indicator.transform.localPosition.x,
                indicator.transform.localPosition.y + Config.ValueIndicatorSize / 2,
                0);
        }

        public Vector2 CalculateCurvePointForPosition (float position, int dataSetIndex) {
            return TransformViewPointIntoAxisPoint (CalculateCurveViewPointForPosition (position, dataSetIndex));
        }

        public Vector2[] CalculateCurvePointsForPositions (float[] positions, int dataSetIndex) {
            Vector2[] result = new Vector2[positions.Length];
            for (int i = 0; i < positions.Length; i++) {
                result[i] = CalculateCurveViewPointForPosition (positions[i], dataSetIndex);
            }

            return TransformViewPointsIntoAxisPoints (result);
        }

        private Vector2 CalculateCurveViewPointForPosition (float position, int dataSetIndex) {
            Vector2[] points = CalculateBezierSegmentsPoints (bezierPoints[dataSetIndex]);

            float viewPosition = position * (1 / GetPositionAxisViewScale ());
            int leftPointIndex = -1;
            for (int i = 1; i < points.Length; i++) {
                if (points[i].x > viewPosition) {
                    leftPointIndex = i - 1;
                    break;
                }
            }

            if (leftPointIndex < 0) {
                return Vector2.zero;
            }

            Vector2 leftPoint = points[leftPointIndex];
            Vector2 rightPoint = points[leftPointIndex + 1];
            float distanceDelta = rightPoint.x - leftPoint.x;
            float valueDelta = rightPoint.y - leftPoint.y;
            float deltaFactor = (viewPosition - leftPoint.x) / distanceDelta;

            return new Vector2 (viewPosition, leftPoint.y + (valueDelta * deltaFactor));
        }

        private float GetPositionAxisViewScale () {
            float positionDelta = GetAxisBounds ().XMax - GetAxisBounds ().XMin;
            return positionDelta / GetSize ().x;
        }

        private float GetValueAxisViewScale () {
            float valueDelta = GetAxisBounds ().YMax - GetAxisBounds ().YMin;
            return valueDelta / GetSize ().y;
        }

        private Vector2 TransformViewPointIntoAxisPoint (Vector2 point) {
            return new Vector2 (point.x * GetPositionAxisViewScale (),
                point.y * GetValueAxisViewScale ());
        }

        private Vector2 TransformAxisPointIntoViewPoint (Vector2 point) {
            return new Vector2 (point.x * (1 / GetPositionAxisViewScale ()),
                point.y * (1 / GetValueAxisViewScale ()));
        }

        private Vector2[] TransformViewPointsIntoAxisPoints (Vector2[] points) {
            float positionScale = GetPositionAxisViewScale ();
            float valueScale = GetValueAxisViewScale ();

            Vector2[] result = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++) {
                result[i] = new Vector2 (points[i].x * positionScale, points[i].y * valueScale);
            }

            return result;
        }
    }
}