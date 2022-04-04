using System.Collections.Generic;

namespace AwesomeCharts {
    [System.Serializable]
    public class BarAxisValueFormatter : AxisValueFormatter {

        public BarAxisValueFormatterConfig config = new BarAxisValueFormatterConfig ();

        public string FormatAxisValue (int index, float value, float minValue, float maxValue) {
            if (ShouldShowCustomValue (config.CustomValues)) {
                return GetCustomValueForIndex (config.CustomValues, index);
            } else {
                return index.ToString ();
            }
        }

        private bool ShouldShowCustomValue (List<string> customValues) {
            return customValues != null && customValues.Count > 0;
        }

        private string GetCustomValueForIndex (List<string> customValues, int index) {
            return customValues.Count > index? customValues[index]: "";
        }
    }
}