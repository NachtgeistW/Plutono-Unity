using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class LineData : AxisChartData {

        [SerializeField]
        private List<LineDataSet> dataSets;

        public List<LineDataSet> DataSets {
            get { return dataSets; }
        }

        public LineData () {
            this.dataSets = new List<LineDataSet> ();
        }

        public LineData (LineDataSet dataSet) : this () {
            dataSets.Add (dataSet);
        }

        public void Clear () {
            dataSets.Clear ();
        }

        public bool HasAnyData () {
            return dataSets.Count > 0;
        }

        public override float GetMinPosition () {
            return HasAnyData () ? DataSets.Select ((dataSet) => dataSet.GetMinPosition ()).Min () :
                0;
        }

        public override float GetMaxPosition () {
            return HasAnyData () ? DataSets.Select ((dataSet) => dataSet.GetMaxPosition ()).Max () :
                0;
        }

        public override float GetMinValue () {
            return HasAnyData () ? DataSets.Select ((dataSet) => dataSet.GetMinValue ()).Min () :
                0;
        }

        public override float GetMaxValue () {
            return HasAnyData () ? DataSets.Select ((dataSet) => dataSet.GetMaxValue ()).Max () :
                0;
        }
    }
}