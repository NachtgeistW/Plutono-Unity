namespace AwesomeCharts {
    public class StringUtils {
        public static string FormatValue (float value, int decimalPlaces) {
            return value.ToString ("F" + decimalPlaces);
        }
    }
}