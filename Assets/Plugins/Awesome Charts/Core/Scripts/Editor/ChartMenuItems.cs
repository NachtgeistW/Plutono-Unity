using UnityEditor;
using UnityEngine;

namespace AwesomeCharts {

    public class ChartMenuItems {

        [MenuItem ("GameObject/UI/BarChart")]
        private static void AddBarChartOption () {
            GameObject gameObject = new GameObject ("BarChart");
            gameObject.AddComponent<BarChart> ();
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 200f);
        }

        [MenuItem ("GameObject/UI/BarChart - TextMeshPro")]
        private static void AddBarChartTMProOption () {
            GameObject gameObject = new GameObject ("BarChart - TMPro");
            BarChart barchart = gameObject.AddComponent<BarChart> ();
            barchart.AxisConfig.HorizontalAxisConfig.AxisLabelPrefab = Resources.Load<ChartLabel> ("prefabs/TMProAxisLabelPrefab");
            barchart.AxisConfig.VerticalAxisConfig.AxisLabelPrefab = Resources.Load<ChartLabel> ("prefabs/TMProAxisLabelPrefab");
            barchart.Config.PopupPrefab = Resources.Load<ChartValuePopup> ("prefabs/TMProChartValuePopup");

            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 200f);
        }

        [MenuItem ("GameObject/UI/LineChart")]
        private static void AddLineChartOption () {
            GameObject gameObject = new GameObject ("LineChart");
            gameObject.AddComponent<LineChart> ();
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 200f);
        }

        [MenuItem ("GameObject/UI/LineChart - TextMeshPro")]
        private static void AddLineChartTMProOption () {
            GameObject gameObject = new GameObject ("LineChart - TMPro");
            LineChart linechart = gameObject.AddComponent<LineChart> ();
            linechart.AxisConfig.HorizontalAxisConfig.AxisLabelPrefab = Resources.Load<ChartLabel> ("prefabs/TMProAxisLabelPrefab");
            linechart.AxisConfig.VerticalAxisConfig.AxisLabelPrefab = Resources.Load<ChartLabel> ("prefabs/TMProAxisLabelPrefab");
            linechart.Config.PopupPrefab = Resources.Load<ChartValuePopup> ("prefabs/TMProChartValuePopup");

            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300f, 200f);
        }

        [MenuItem ("GameObject/UI/PieChart")]
        private static void AddPieChartOption () {
            GameObject gameObject = new GameObject ("PieChart");
            PieChart pieChart = gameObject.AddComponent<PieChart> ();
            pieChart.Config.ValueLabelPrefab = Resources.Load<ChartLabel> ("prefabs/PieChartIndicatorLabelPrafab");
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200f, 200f);
        }

        [MenuItem ("GameObject/UI/PieChart - TextMeshPro")]
        private static void AddPieChartTMProOption () {
            GameObject gameObject = new GameObject ("PieChart - TMPro");
            PieChart pieChart = gameObject.AddComponent<PieChart> ();
            pieChart.Config.ValueLabelPrefab = Resources.Load<ChartLabel> ("prefabs/PieChartIndicatorLabelTMProPrefab");
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200f, 200f);
        }

        [MenuItem ("GameObject/UI/LegendView")]
        private static void AddLegendViewOption () {
            GameObject gameObject = new GameObject ("LegendView");
            gameObject.AddComponent<LegendView> ();
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f, 100f);
        }

        [MenuItem ("GameObject/UI/LegendView - TextMeshPro")]
        private static void AddLegendViewTMProOption () {
            GameObject gameObject = new GameObject ("LegendView - TMPro");
            LegendView legendView = gameObject.AddComponent<LegendView> ();
            legendView.entryViewPrefab = Resources.Load<LegendEntryView> ("prefabs/TMProLegendEntryView");
            if (Selection.transforms.Length > 0) {
                gameObject.GetComponent<RectTransform> ().SetParent (Selection.transforms[0], false);
            }
            gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f, 100f);
        }
    }
}