using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {
    public abstract class LinearAxisLabelEntryProvider : AxisLabelEntryProvider {

        public int labelCount = 0;
        public float axisLength = 0f;
        public float valueMin = 0f;
        public float valueMax = 0f;
        public bool firstEntryVisible = true;
        public bool lastEntryVisible = true;
        public AxisLabelGravity labelsGravity = AxisLabelGravity.START;
        public AxisValueFormatter valueFormatter = new BasicAxisValueFormatter ();

        protected abstract AxisOrientation GetEntryAxisOrientation ();

        protected virtual float GetLabelAxisPosition (int index, int maxIndex) {
            return axisLength > 0f? axisLength * (index / (float) maxIndex) : 0f;
        }

        public List<AxisLabelRendererExtry> getLabelRendererEntries () {
            List<AxisLabelRendererExtry> entries = new List<AxisLabelRendererExtry> ();
            int minLabelIndex = GetMinLabelIndex ();
            int maxLabelIndex = GetMaxLabelIndex ();
            for (int i = minLabelIndex; i < minLabelIndex + labelCount; i++) {
                AxisLabelRendererExtry entry = new AxisLabelRendererExtry ();
                entry.PositionOnAxis = GetLabelAxisPosition (i, maxLabelIndex);
                entry.Gravity = labelsGravity;
                entry.Text = GetLabelValueText (i - minLabelIndex, entry.PositionOnAxis);
                entry.Orientation = GetEntryAxisOrientation ();
                entries.Add (entry);
            }

            return entries;
        }

        private int GetMinLabelIndex () {
            return firstEntryVisible ? 0 : 1;
        }

        private int GetMaxLabelIndex () {
            int result = GetMinLabelIndex () + labelCount - 1;
            if (!lastEntryVisible)
                result++;

            return result;
        }

        private string GetLabelValueText (int index, float axisPosition) {
            float axisValue = 0f;
            float valueDiff = valueMax - valueMin;
            if (valueDiff > 0f && axisLength > 0f) {
                axisValue = valueMin + (valueDiff * (axisPosition / axisLength));
            }

            return valueFormatter.FormatAxisValue (index, axisValue, valueMin, valueMax);
        }
    }
}