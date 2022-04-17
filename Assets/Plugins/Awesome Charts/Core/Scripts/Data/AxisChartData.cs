using System;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {

    [Serializable]
    public abstract class AxisChartData : ChartData {

        public abstract float GetMinPosition();

        public abstract float GetMaxPosition();

        public abstract float GetMinValue();

        public abstract float GetMaxValue();
    }
}