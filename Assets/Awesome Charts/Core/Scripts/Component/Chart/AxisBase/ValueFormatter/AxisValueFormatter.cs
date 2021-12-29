namespace AwesomeCharts {
    public interface AxisValueFormatter {
        string FormatAxisValue (int index, float value, float minValue, float maxValue);
    }
}