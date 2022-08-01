using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class BarData : AxisChartData {

        [SerializeField]
        private List<BarDataSet> dataSets;

        public List<BarDataSet> DataSets {
            get { return dataSets; }
        }

        public BarData () {
            dataSets = new List<BarDataSet> ();
        }

        public BarData (BarDataSet dataSet) : this () {
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
            return 0;
        }

        public override float GetMaxValue () {
            return HasAnyData () ? DataSets.Select ((dataSet) => dataSet.GetMaxValue ()).Max () :
                0;
        }
    }
}