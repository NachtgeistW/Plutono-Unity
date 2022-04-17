using System.Collections.Generic;

namespace AwesomeCharts {
    [System.Serializable]
    public class BasicAxisValueFormatter : AxisValueFormatter {

        public BasicValueFormatterConfig config = new BasicValueFormatterConfig ();

        public string FormatAxisValue (int index, float value, float minValue, float maxValue) {
            if (ShouldShowCustomValue (config.CustomValues)) {
                return GetCustomValueForIndex (config.CustomValues, index);
            } else {
                return GetFormattedValue (value, config.ValueDecimalPlaces);
            }
        }

        private bool ShouldShowCustomValue (List<string> customValues) {
            return customValues != null && customValues.Count > 0;
        }

        private string GetCustomValueForIndex (List<string> customValues, int index) {
            return customValues.Count > index? customValues[index]: "";
        }

        private string GetFormattedValue (float value, int decimalPlaces) {
            return StringUtils.FormatValue (value, decimalPlaces);
        }
    }
}