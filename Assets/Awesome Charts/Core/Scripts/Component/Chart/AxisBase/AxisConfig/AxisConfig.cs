using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public abstract class AxisConfig<V, H>
        where V : SingleAxisConfig
    where H : SingleAxisConfig {

        [SerializeField]
        private V verticalAxisConfig;
        [SerializeField]
        private H horizontalAxisConfig;

        protected abstract V CreateDefaultVerticalAxis();

        protected abstract H CreateDefaultHorizontalAxis();

        public AxisConfig(){
            verticalAxisConfig = CreateDefaultVerticalAxis();
            horizontalAxisConfig = CreateDefaultHorizontalAxis();
        }

        public V VerticalAxisConfig {
            get { return verticalAxisConfig; }
            set { verticalAxisConfig = value; }
        }

        public H HorizontalAxisConfig {
            get { return horizontalAxisConfig; }
            set { horizontalAxisConfig = value; }
        }
    }
}