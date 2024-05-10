using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexagonalMapConfig))]
public class HexagonalMapConfigEditor : Editor {
    private SerializedProperty totalRingsProp;
    private SerializedProperty hexFieldsProp;
    private SerializedProperty numberFieldsProp; // Serialized property for numberFields

    private void OnEnable() {
        totalRingsProp = serializedObject.FindProperty("totalRings");
        hexFieldsProp = serializedObject.FindProperty("hexFields");
        numberFieldsProp = serializedObject.FindProperty("numberFields"); // Initialize the serialized property for numberFields
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(totalRingsProp);

        if (totalRingsProp.intValue < 0) {
            EditorGUILayout.HelpBox("Total Rings should be a non-negative integer.", MessageType.Error);
            totalRingsProp.intValue = 0;
        }

        serializedObject.ApplyModifiedProperties();

        UpdateHexFields(totalRingsProp.intValue);

        // Ensure hexFieldsProp and numberFieldsProp are not null
        if (hexFieldsProp != null && numberFieldsProp != null) {
            // Draw the hexFields and corresponding numberFields
            for (int i = 0; i < hexFieldsProp.arraySize; i++) {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty hexFieldProp = hexFieldsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(hexFieldProp);
                SerializedProperty numberFieldProp = numberFieldsProp.GetArrayElementAtIndex(i); // Get the corresponding number field
                EditorGUILayout.PropertyField(numberFieldProp, GUIContent.none, GUILayout.Width(40)); // Display number field without label
                EditorGUILayout.EndHorizontal();
            }

            // Apply changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void UpdateHexFields(int numRings) {
        int currentFields = hexFieldsProp.arraySize;
        int targetFields = CalculateTotalFields(numRings);

        if (currentFields != targetFields) {
            // Clear or add elements to the hexFields and numberFields arrays based on the number of rings
            if (targetFields > currentFields) {
                int numToAdd = targetFields - currentFields;
                for (int i = 0; i < numToAdd; i++) {
                    hexFieldsProp.InsertArrayElementAtIndex(hexFieldsProp.arraySize);
                    numberFieldsProp.InsertArrayElementAtIndex(numberFieldsProp.arraySize); // Add corresponding number field
                }
            } else {
                hexFieldsProp.arraySize = targetFields;
                numberFieldsProp.arraySize = targetFields; // Adjust numberFields array size
            }
        }
    }

    private int CalculateTotalFields(int numRings) {
        // Calculate the total number of fields based on the number of rings
        int totalFields = 0;
        for (int ring = 0; ring < numRings; ring++) {
            totalFields += 6 * (ring + 1);
        }
        return totalFields;
    }
}
