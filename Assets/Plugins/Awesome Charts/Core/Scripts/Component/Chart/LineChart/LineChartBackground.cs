using System;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {
    public class LineChartBackground : MaskableGraphic {

        [SerializeField]
        private Vector2[] points;
        [SerializeField]
        private AxisBounds axisBounds;
        [SerializeField]
        private Texture texture;

        public Vector2[] Points {
            get { return points; }
            set {
                points = value;
                SetAllDirty();
            }
        }

        public AxisBounds AxisBounds {
            get { return axisBounds; }
            set {
                axisBounds = value;
                SetAllDirty();
            }
        }
               
        public Texture Texture {
            get {
                return texture;
            }
            set {
                if (texture == value)
                    return;

                texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public override Texture mainTexture {
            get {
                return texture ?? s_WhiteTexture;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            if (Points.Length < 2)
                return;

            Vector2 prevTop = points[0];
            Vector2 prevBottom = new Vector2(points[0].x, 0f);

            float maxX = 0f;
            float maxY = 0f;

            foreach(Vector2 point in points) { 
                if(point.x > maxX) {
                    maxX = point.x;
                }
                if(point.y > maxY) {
                    maxY = point.y;
                }
            }

            for (int i = 1; i < Points.Length; i++) {
                Vector2 currentTop = points[i];
                Vector2 currentBottom = new Vector2(points[i].x, 0f);

                vh.AddUIVertexQuad(CreateUIVertices(
                    new[] { prevBottom, prevTop, currentTop, currentBottom },
                    new[] { GetCorrectUV(prevBottom, maxX, maxY),
                            GetCorrectUV(prevTop, maxX, maxY),
                            GetCorrectUV(currentTop, maxX, maxY),
                            GetCorrectUV(currentBottom, maxX, maxY)}));

                prevTop = currentTop;
                prevBottom = currentBottom;
            }
        }

        private Vector2 GetCorrectUV(Vector2 point, float maxX, float maxY) {
            return new Vector2(point.x/maxX, point.y/maxY);
        }

        private UIVertex[] CreateUIVertices(Vector2[] vertices, Vector2[] uvs) {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++) {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }
    }
}
