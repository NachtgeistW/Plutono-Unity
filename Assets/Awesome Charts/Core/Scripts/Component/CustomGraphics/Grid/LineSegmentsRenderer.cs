using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {
    [RequireComponent (typeof (RectTransform))]
    public class LineSegmentsRenderer : BaseMaskableGraphic {

        protected Vector2 GetSize () {
            Rect rect = GetComponent<RectTransform> ().rect;
            return new Vector2 (rect.width, rect.height);
        }

        protected void DrawSegments (VertexHelper vh, List<LineSegment> segments, GridLineConfig config) {
            segments.ForEach (segment => {
                vh.AddUIVertexQuad (CreateUIVertices (
                    segment.CreateSegmentVertices (),
                    CreateDefaultUVs (),
                    config.Color
                ));
            });
        }

        protected List<LineSegment> CreateVerticalOrHorizontalSegments (int linesCount, GridLineConfig config, float width, float height, bool vertical) {
            return CreateSplitLinePoints (linesCount, height)
                .SelectMany (point => CreateLineSegments (point, config, width, vertical)).ToList ();
        }

        protected List<float> CreateSplitLinePoints (int pointsCount, float lineLenght) {
            float pointDistance = lineLenght / (pointsCount + 1);
            List<float> resultPoints = new List<float> ();
            for (int i = 0; i < pointsCount; i++) {
                resultPoints.Add (pointDistance * (i + 1));
            }

            return resultPoints;
        }

        protected List<LineSegment> CreateLineSegments (float startingPoint,
            GridLineConfig config,
            float totalLenght,
            bool vertical) {

            float segmentLenght = config.ShouldDrawDashedLines () ? config.DashLenght : totalLenght;
            float segmentSpacing = config.ShouldDrawDashedLines () ? config.DashSpacing : 0;
            float a = startingPoint;
            float b = 0f;
            List<LineSegment> result = new List<LineSegment> ();

            while (b < totalLenght) {
                float currentSegmentLenght = calculateSegmentLength (b, segmentLenght, segmentSpacing, totalLenght);
                Vector2 startingVector = new Vector2 (vertical? a : b, vertical? b : a);
                result.Add (CreateLine (startingVector, config.Thickness, currentSegmentLenght, vertical));
                b += currentSegmentLenght + segmentSpacing;
            }

            return result;
        }

        protected float calculateSegmentLength (float startingPoint,
            float desiredSegmentLenght,
            float segmentSpacing,
            float totalLenght) {
            float leftSpace = totalLenght - startingPoint;

            if (desiredSegmentLenght + segmentSpacing < leftSpace)
                return desiredSegmentLenght;
            else
                return leftSpace;
        }

        protected LineSegment CreateLine (Vector2 startingPoint, int thickness, float lenght, bool vertical) {
            Vector2 bottomLeft = vertical?
            new Vector2 (startingPoint.x - thickness / 2f, startingPoint.y):
                new Vector2 (startingPoint.x, startingPoint.y - thickness / 2f);

            Vector2 topRight = vertical?
            new Vector2 (startingPoint.x + thickness / 2f, startingPoint.y + lenght):
                new Vector2 (startingPoint.x + lenght, startingPoint.y + thickness / 2f);

            return new LineSegment (bottomLeft, topRight);
        }
    }
}