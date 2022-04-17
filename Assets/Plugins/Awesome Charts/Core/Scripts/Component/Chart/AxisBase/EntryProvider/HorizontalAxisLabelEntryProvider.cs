using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class HorizontalAxisLabelEntryProvider : LinearAxisLabelEntryProvider {

        protected override AxisOrientation GetEntryAxisOrientation () {
            return AxisOrientation.HORIZONTAL;
        }
    }
}