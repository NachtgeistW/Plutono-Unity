using System.Collections;
using System.Collections.Generic;
using AwesomeCharts;
using UnityEngine;

[ExecuteInEditMode]
public class CustomAxisValueFormatterExample : MonoBehaviour {

    public BarChart barChart;

    void Start () {
        SetupCustomValueFormatter ();
    }

    private void SetupCustomValueFormatter () {
        if (barChart == null)
            return;

        barChart.CustomVerticalAxisValueFormatter = new PriceValueFormatter ();
    }

    private class PriceValueFormatter : AxisValueFormatter {
        public string FormatAxisValue (int index, float value, float minValue, float maxValue) {
            return value.ToString ("F" + 0) + " $";
        }
    }
}