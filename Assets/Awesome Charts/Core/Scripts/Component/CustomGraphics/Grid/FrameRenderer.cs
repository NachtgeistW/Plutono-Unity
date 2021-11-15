using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    public class FrameRenderer : LineSegmentsRenderer {
        [SerializeField]
        private GridFrameConfig gridFrameConfig = new GridFrameConfig ();

        public GridFrameConfig GridFrameConfig {
            get { return gridFrameConfig; }
            set {
                gridFrameConfig = value;
                SetAllDirty ();
            }
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();

            Vector2 size = GetSize ();
            DrawGridFrame (vh, size, gridFrameConfig);
        }

        private void DrawGridFrame (VertexHelper vh, Vector2 size, GridFrameConfig frameConfig) {
            if (frameConfig == null)
                return;

            float halfLineThicness = frameConfig.LinesConfig.Thickness / 2f;
            if (frameConfig.DrawLeftLine) {
                DrawSegments (vh,
                    CreateLineSegments (halfLineThicness, frameConfig.LinesConfig, size.y, true),
                    frameConfig.LinesConfig);
            }

            if (frameConfig.DrawRightLine) {
                DrawSegments (vh,
                    CreateLineSegments (size.x - halfLineThicness, frameConfig.LinesConfig, size.y, true),
                    frameConfig.LinesConfig);
            }

            if (frameConfig.DrawBottomLine) {
                DrawSegments (vh,
                    CreateLineSegments (halfLineThicness, frameConfig.LinesConfig, size.x, false),
                    frameConfig.LinesConfig);
            }

            if (frameConfig.DrawTopLine) {
                DrawSegments (vh,
                    CreateLineSegments (size.y - halfLineThicness, frameConfig.LinesConfig, size.x, false),
                    frameConfig.LinesConfig);
            }
        }
    }
}