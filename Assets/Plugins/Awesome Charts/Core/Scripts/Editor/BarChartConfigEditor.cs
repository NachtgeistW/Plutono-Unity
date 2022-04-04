using UnityEditor;
using UnityEngine;

namespace AwesomeCharts {
    [CustomPropertyDrawer(typeof(BarChartConfig))]
    public class BarChartConfigEditor : PropertyDrawer {
        protected const float LINE_HEIGHT = 18f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, label);

            Rect labelRect = new Rect(position.x + 15f, position.y, 70f, EditorGUIUtility.singleLineHeight);
            Rect contentRect = new Rect(labelRect.x + labelRect.width, labelRect.y, position.width - labelRect.width - 15f, EditorGUIUtility.singleLineHeight);

            float currentY = labelRect.y;
            currentY += LINE_HEIGHT;

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60f;

            EditorGUI.LabelField(new Rect(labelRect.x, currentY, labelRect.width, labelRect.height), "Bar: ");

            currentY = SetupBarProperties(property, contentRect, currentY);
            currentY += LINE_HEIGHT;

            EditorGUI.LabelField(new Rect(labelRect.x, currentY, labelRect.width, labelRect.height), "Bar spacing: ");

            currentY = SetupBarSpecingProperties(property, contentRect, currentY);
            currentY += LINE_HEIGHT;

            EditorGUI.LabelField(new Rect(labelRect.x, currentY, labelRect.width, labelRect.height), "Prefabs: ");

            currentY = SetupPrefabProperties(property, contentRect, currentY);

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 5 * LINE_HEIGHT;
        }

        protected virtual float SetupBarProperties(SerializedProperty property, Rect contentRect, float positionY) {
            EditorGUI.PropertyField(new Rect(contentRect.x, positionY, contentRect.width / 2, contentRect.height),
            property.FindPropertyRelative("barWidth"), new GUIContent("width"));
            EditorGUI.PropertyField(new Rect(contentRect.x + contentRect.width / 2, positionY, contentRect.width / 2, contentRect.height),
            property.FindPropertyRelative("sizingMethod"), new GUIContent("sizing"));

            return positionY;
        }

        protected virtual float SetupBarSpecingProperties(SerializedProperty property, Rect contentRect, float positionY) {
            EditorGUI.PropertyField(new Rect(contentRect.x, positionY, contentRect.width / 2, contentRect.height),
            property.FindPropertyRelative("barSpacing"), new GUIContent("base"));
            EditorGUI.PropertyField(new Rect(contentRect.x + contentRect.width / 2, positionY, contentRect.width / 2, contentRect.height),
            property.FindPropertyRelative("innerBarSpacing"), new GUIContent("inner"));

            return positionY;
        }

        protected virtual float SetupPrefabProperties(SerializedProperty property, Rect contentRect, float positionY) {
            EditorGUI.PropertyField(new Rect(contentRect.x, positionY, contentRect.width, contentRect.height),
            property.FindPropertyRelative("barPrefab"), new GUIContent("bar"));

            positionY += LINE_HEIGHT;

            EditorGUI.PropertyField(new Rect(contentRect.x, positionY, contentRect.width, contentRect.height),
            property.FindPropertyRelative("popupPrefab"), new GUIContent("popup"));

            return positionY;
        }
    }
}
