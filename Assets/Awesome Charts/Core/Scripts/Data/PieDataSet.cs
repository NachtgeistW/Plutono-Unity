using System;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class PieDataSet : DataSet<PieEntry> {

        private float totalValue = 0;
        private List<float> calculatedPercentValues = new List<float> ();
        private List<float> calculatedRotationValues = new List<float> ();

        [SerializeField]
        private Boolean valuesAsPercentages = false;

        public Boolean ValuesAsPercentages {
            get { return valuesAsPercentages; }
            set {
                valuesAsPercentages = value;
                RecalculateValues ();
            }
        }

        public PieDataSet () : this ("") { }

        public PieDataSet (string title) : base (title) { }

        public PieDataSet (string title, List<PieEntry> entries) : base (title, entries) { }

        internal void RecalculateValues () {
            OnEntriesChanged ();
        }

        protected override void OnEntriesChanged () {
            totalValue = CalculateTotalValue ();
            calculatedPercentValues = CalculatePercentValues ();
            calculatedRotationValues = CalculateRotationValues ();
        }

        private float CalculateTotalValue () {
            float result = 0f;

            if (ValuesAsPercentages) {
                result = 100f;
            } else {
                foreach (PieEntry entry in Entries) {
                    result += entry.Value;
                }
            }

            return result;
        }

        private List<float> CalculatePercentValues () {
            List<float> result = new List<float> ();
            for (int i = 0; i < GetEntriesCount (); i++) {
                result.Add (Entries[i].Value / totalValue);
            }
            return result;
        }

        private List<float> CalculateRotationValues () {
            List<float> result = new List<float> ();
            float currentRotation = 0f;
            for (int i = 0; i < GetEntriesCount (); i++) {
                result.Add (currentRotation * 360f);
                currentRotation += calculatedPercentValues[i];
            }
            return result;
        }

        public override List<PieEntry> GetSortedEntries () {
            return Entries;
        }

        public float GetTotalValue () {
            return totalValue;
        }

        public float GetPercentValue (int index) {
            if (calculatedPercentValues.Count > index) {
                return calculatedPercentValues[index];
            }
            return 0f;
        }

        public float GetRotationValue (int index) {
            if (calculatedRotationValues.Count > index) {
                return calculatedRotationValues[index];
            }
            return 0f;
        }

        public int EntryIndexForAngle (double angle) {
            for (int i = 0; i < GetEntriesCount (); i++) {
                if (GetRotationValue (i) + (GetPercentValue (i) * 360f) > angle) {
                    return i;
                }
            }
            return -1;
        }
    }
}