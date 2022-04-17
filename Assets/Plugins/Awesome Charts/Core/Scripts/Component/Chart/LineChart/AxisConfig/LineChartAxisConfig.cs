using System;
using UnityEngine;

namespace AwesomeCharts {
    [Serializable]
    public class LineChartAxisConfig : AxisConfig<LinearSingleAxisConfig, LinearSingleAxisConfig> {

        protected override LinearSingleAxisConfig CreateDefaultHorizontalAxis () {
            return new LinearSingleAxisConfig ();
        }

        protected override LinearSingleAxisConfig CreateDefaultVerticalAxis () {
            return new LinearSingleAxisConfig ();
        }
    }
}