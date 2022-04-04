using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class VerticalAxisLabelEntryProvider : LinearAxisLabelEntryProvider {

        protected override AxisOrientation GetEntryAxisOrientation () {
            return AxisOrientation.VERTICAL;
        }
    }
}