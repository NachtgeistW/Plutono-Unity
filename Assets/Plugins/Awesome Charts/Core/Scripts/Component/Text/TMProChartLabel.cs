using TMPro;
using UnityEngine;

namespace AwesomeCharts {
    public class TMProChartLabel : ChartLabel {

        public TMP_Text textLabel;

        public override void SetLabelColor (Color color) {
            textLabel.color = color;
        }

        public override void SetLabelText (string text) {
            textLabel.text = text;
        }

        public override void SetLabelTextAlignment (TextAnchor anchor) {
            textLabel.alignment = GetTextMeshProAligment(anchor);
        }

        private TextAlignmentOptions GetTextMeshProAligment (TextAnchor anchor) {
            switch (anchor) {
                case TextAnchor.LowerCenter:
                    return TextAlignmentOptions.Bottom;
                case TextAnchor.LowerLeft:
                    return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight:
                    return TextAlignmentOptions.BottomRight;
                case TextAnchor.MiddleCenter:
                    return TextAlignmentOptions.Midline;
                case TextAnchor.MiddleLeft:
                    return TextAlignmentOptions.MidlineLeft;
                case TextAnchor.MiddleRight:
                    return TextAlignmentOptions.MidlineRight;
                case TextAnchor.UpperCenter:
                    return TextAlignmentOptions.Top;
                case TextAnchor.UpperLeft:
                    return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperRight:
                    return TextAlignmentOptions.TopRight;
                default:
                    return TextAlignmentOptions.Midline;

            }
        }

        public override void SetLabelTextSize (int size) {
            textLabel.fontSize = size;
        }
    }
}