using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {

    public class BaseMaskableGraphic : MaskableGraphic {

        protected Vector2[] CreateDefaultUVs () {
            return new [] {
                new Vector2 (0f, 0f),
                    new Vector2 (0f, 1f),
                    new Vector2 (1f, 1f),
                    new Vector2 (1f, 0f)
            };
        }

        protected UIVertex[] CreateUIVertices (Vector2[] vertices, Vector2[] uvs, Color color) {
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