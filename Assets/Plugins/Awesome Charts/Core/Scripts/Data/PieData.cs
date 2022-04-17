using System;
using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class PieData : ChartData {

        [SerializeField]
        private PieDataSet dataSet;

        public PieDataSet DataSet {
            get { return dataSet; }
            set { dataSet = value; }
        }

        public PieData() {
            dataSet = new PieDataSet();
        }

        public PieData(PieDataSet dataSet) {
            this.DataSet = dataSet;
        }

        public void Clear(){
            dataSet.Clear();
        }
    }
}
