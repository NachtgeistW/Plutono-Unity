using System;
using UnityEngine;

namespace AwesomeCharts {
    public class MathUtils {

        public static double GetAngle (Vector2 from, Vector2 to) {
            return Math.Atan2 (to.y - from.y, to.x - from.x) * (180 / Math.PI);
        }

        public static double AngleToCircleAngle (double angle) {
            if (angle >= 0.0 && angle < 90.0) {
                return 90 - angle;
            } else if (angle >= 90 && angle <= 180) {
                return 450 - angle;
            } else {
                return 90 - angle;
            }
        }

        public static Vector2 GetPositionOnCircle (float angle, float radius) {
            float radians = angle * Mathf.Deg2Rad;
            Vector2 vector = new Vector2 (Mathf.Sin (radians), Mathf.Cos (radians));
            return vector * radius;
        }
    }
}