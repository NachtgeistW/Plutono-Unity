using UnityEngine;

namespace AwesomeCharts {
    public class LineSegment {

        public Vector2 bottomLeft;
        public Vector2 topRight;

        public LineSegment () {
            bottomLeft = Vector2.zero;
            topRight = Vector2.zero;
        }

        public LineSegment (Vector2 bottomLeft, Vector2 topRight) {
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
        }

        public Vector2[] CreateSegmentVertices () {
            return new [] { bottomLeft, GetTopLeft (), topRight, GetBottomRight () };
        }

        public Vector2 GetTopLeft () {
            return new Vector2 (bottomLeft.x, topRight.y);
        }

        public Vector2 GetBottomRight () {
            return new Vector2 (topRight.x, bottomLeft.y);
        }
    }
}