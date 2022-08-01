using System;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {

    public static class BezierUtils {

        public static Vector2[] CreateBezierPointsFromLinePoints(Vector2[] points) {
            Vector2[] firstControlPoints;
            Vector2[] secondControlPoints;

            GetCurveControlPoints(points, out firstControlPoints, out secondControlPoints);

            Vector2[] result = new Vector2[(points.Length * 3) - 2];
            for (int i = 0; i < points.Length; i++) {
                result[i * 3] = points[i];
                if (i < points.Length - 1) {
                    result[(i * 3) + 1] = firstControlPoints[i];
                    result[(i * 3) + 2] = secondControlPoints[i];
                }
            }

            return result;
        }

        public static void GetCurveControlPoints(Vector2[] knots,
        out Vector2[] firstControlPoints, out Vector2[] secondControlPoints) {
            if (knots == null)
                throw new ArgumentNullException("knots");
            int n = knots.Length - 1;
            if (n < 1)
                throw new ArgumentException
                ("At least two knot points required", "knots");
            if (n == 1) { 
                firstControlPoints = new Vector2[1];
                firstControlPoints[0].x = (2 * knots[0].x + knots[1].x) / 3;
                firstControlPoints[0].y = (2 * knots[0].y + knots[1].y) / 3;

                secondControlPoints = new Vector2[1];
                secondControlPoints[0].x = 2 *
                    firstControlPoints[0].x - knots[0].x;
                secondControlPoints[0].y = 2 *
                    firstControlPoints[0].y - knots[0].y;
                return;
            }
             
            float[] valuesX = knots.Select(knot => knot.x).ToArray();
            float[] valuesY = knots.Select(knot => knot.y).ToArray();
            float[] x = GetFirstControlPoints(CreateRightHandVectors(valuesX));
            float[] y = GetFirstControlPoints(CreateRightHandVectors(valuesY));

            firstControlPoints = new Vector2[n];
            secondControlPoints = new Vector2[n];
            for (int i = 0; i < n; ++i) {
                firstControlPoints[i] = new Vector2(x[i], y[i]);
                if (i < n - 1)
                    secondControlPoints[i] = new Vector2(2 * knots
                        [i + 1].x - x[i + 1], 2 *
                        knots[i + 1].y - y[i + 1]);
                else
                    secondControlPoints[i] = new Vector2((knots
                        [n].x + x[n - 1]) / 2,
                        (knots[n].y + y[n - 1]) / 2);
            }
        }

        private static float[] CreateRightHandVectors(float[] knotValues){
            int n = knotValues.Length - 1;
            float[] rightVectors = new float[n];

            for (int i = 1; i < n - 1; ++i)
                rightVectors[i] = 4 * knotValues[i] + 2 * knotValues[i + 1];

            rightVectors[0] = knotValues[0] + 2 * knotValues[1];
            rightVectors[n - 1] = (8 * knotValues[n - 1] + knotValues[n]) / 2.0f;
            return rightVectors;
        }

        private static float[] GetFirstControlPoints(float[] rightVectors) {
            int length = rightVectors.Length;
            float[] result = new float[length]; 
            float[] tmp = new float[length]; 

            float b = 2.0f;
            result[0] = rightVectors[0] / b;
            for (int i = 1; i < length; i++) 
            {
                tmp[i] = 1 / b;
                b = (i < length - 1 ? 4.0f : 3.5f) - tmp[i];
                result[i] = (rightVectors[i] - result[i - 1]) / b;
            }
            for (int i = 1; i < length; i++)
                result[length - i - 1] -= tmp[length - i] * result[length - i];

            return result;
        }
    }
}
