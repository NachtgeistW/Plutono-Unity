using UnityEngine;

namespace AwesomeCharts {
    public abstract class ChartLabel : MonoBehaviour {

        public abstract void SetLabelText(string text);

        public abstract void SetLabelTextSize(int size);

        public abstract void SetLabelTextAlignment(TextAnchor anchor);

        public abstract void SetLabelColor(Color color);
    }
}