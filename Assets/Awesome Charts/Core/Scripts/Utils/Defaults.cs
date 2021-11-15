using System;
using UnityEngine;

namespace AwesomeCharts {
    public static class Defaults {

        public const int AXIS_LINE_THICKNESS = 2;
        public const int AXIS_LINES_COUNT = 5;
        public const int AXIS_LABELS_COUNT = 5;
        public const int AXIS_LABEL_SIZE = 14;
        public const float AXIS_LABEL_MARGIN = 5.0f;
        public const float AXIS_MIN_VALUE = 0.0f;
        public const float AXIS_MAX_VALUE = 100.0f;
        public static Color AXIS_LINE_COLOR = Color.white;
        public static Color AXIS_LABEL_COLOR = Color.white;

        public const int AXIS_RENDERER_LINE_DASH_LENGTH = 8;
        public const int AXIS_RENDERER_LINE_DASH_SPACE = 4;

        public const int BAR_WIDTH = 40;
        public const int BAR_SPACING = 15;
        public const int INNER_BAR_SPACING = 5;
        public const BarSizingMethod BAR_SIZING_METHOD = BarSizingMethod.STANDARD;

        public const float CHART_LINE_THICKNESS = 3f;
        public static Color CHART_LINE_COLOR = Color.white;
        public static Color CHART_BACKGROUND_COLOR = Color.clear;

        public static int LINE_CHART_VALUE_INDICATOR_SIZE = 12;

        public static Color BAR_LINE_COLOR = Color.white;
    }
}
