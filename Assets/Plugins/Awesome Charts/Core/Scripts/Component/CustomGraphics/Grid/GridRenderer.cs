using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    public class GridRenderer : LineSegmentsRenderer {

        [SerializeField]
        private GridConfig gridConfig = new GridConfig ();

        public GridConfig GridConfig {
            get { return gridConfig; }
            set {
                gridConfig = value;
                SetAllDirty ();
            }
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();

            Vector2 size = GetSize ();
            DrawGrid (vh, size);
        }

        private void DrawGrid (VertexHelper vh, Vector2 size) {
            DrawSegments (vh,
                CreateVerticalOrHorizontalSegments (GridConfig.VerticalLinesCount, GridConfig.VerticalLinesConfig, size.y, size.x, true),
                GridConfig.VerticalLinesConfig);

            DrawSegments (vh,
                CreateVerticalOrHorizontalSegments (GridConfig.HorizontalLinesCount, GridConfig.HorizontalLinesConfig, size.x, size.y, false),
                GridConfig.HorizontalLinesConfig);
        }
    }
}