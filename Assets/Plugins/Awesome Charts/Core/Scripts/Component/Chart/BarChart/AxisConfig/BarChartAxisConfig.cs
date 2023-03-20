namespace AwesomeCharts {
    [System.Serializable]
    public class BarChartAxisConfig : AxisConfig<LinearSingleAxisConfig, BarSingleAxisConfig> {

        protected override BarSingleAxisConfig CreateDefaultHorizontalAxis () {
            return new BarSingleAxisConfig ();
        }

        protected override LinearSingleAxisConfig CreateDefaultVerticalAxis () {
            return new LinearSingleAxisConfig ();
        }
    }
}