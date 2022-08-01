using UnityEngine;
using UnityEditor;

namespace AwesomeCharts {
    [CustomPropertyDrawer(typeof(LineEntry))]
    public class LineEntryDrawer : PropertyDrawer {

        private const float HORIZONTAL_SPACING = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            Rect contentRect = EditorGUI.IndentedRect(position);

            float labelWidth = EditorGUIUtility.labelWidth;
            int indentLevel = EditorGUI.indentLevel;

            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 61f;

            EditorGUI.PropertyField(new Rect(contentRect.x, contentRect.y, (contentRect.width / 2) - HORIZONTAL_SPACING, contentRect.height),
            property.FindPropertyRelative("position"), new GUIContent("Position"));

            EditorGUI.PropertyField(new Rect(contentRect.x + contentRect.width / 2, contentRect.y, contentRect.width / 2, contentRect.height),
            property.FindPropertyRelative("value"), new GUIContent("Value"));

            EditorGUI.indentLevel = indentLevel;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.EndProperty();
        }
    }
}
