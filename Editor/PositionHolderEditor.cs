using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionHolder))]
public class PositionHolderEditor : Editor
{
    private PositionHolder positionHolder;
    private SerializedProperty localPositionsProperty;

    private void OnEnable()
    {
        positionHolder = (PositionHolder)target;
        localPositionsProperty = serializedObject.FindProperty("localPositions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(localPositionsProperty, new GUIContent("Local Positions"), true);

        if (GUILayout.Button("Add Position"))
        {
            positionHolder.localPositions.Add(Vector3.zero);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        for (int i = 0; i < positionHolder.localPositions.Count; i++)
        {
            // Convert the local position to world position for display and manipulation
            Vector3 worldPosition = positionHolder.transform.TransformPoint(positionHolder.localPositions[i]);

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(positionHolder, "Move Position Handle");
                // Convert the manipulated world position back to local position
                positionHolder.localPositions[i] = positionHolder.transform.InverseTransformPoint(newWorldPosition);
                EditorUtility.SetDirty(positionHolder);
            }
        }
    }
}