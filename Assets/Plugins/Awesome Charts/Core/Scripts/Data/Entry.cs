using UnityEngine;

namespace AwesomeCharts {

    [System.Serializable]
    public class Entry {

        [SerializeField]
        protected float value;

        public Entry () {
            this.value = 0f;
        }

        public Entry (float value) {
            this.value = value;
        }

        virtual public float Value {
            get { return Mathf.Max (value, 0); }
            set { this.value = value; }
        }
    }
}