using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {
    public class TextChartLabel : ChartLabel {

        public Text textLabel;

        public override void SetLabelColor (Color color) {
            textLabel.color = color;
        }

        public override void SetLabelText (string text) {
            textLabel.text = text;
        }

        public override void SetLabelTextAlignment (TextAnchor anchor) {
            textLabel.alignment = anchor;
        }

        public override void SetLabelTextSize (int size) {
            textLabel.fontSize = size;
        }
    }
}