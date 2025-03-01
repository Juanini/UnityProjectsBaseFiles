using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionHolder))]
public class PositionHolderEditor : Editor
{
    private PositionHolder positionHolder;
    private SerializedProperty localPositionsProperty;

    // Configuración para los gizmos (se mostrará en el inspector del editor)
    public bool drawDebugBox = false;
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);
    public Color color = Color.green;

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
        
        // Opcional: agregar configuración de gizmos en el inspector
        drawDebugBox = EditorGUILayout.Toggle("Draw Debug Box", drawDebugBox);
        boxSize = EditorGUILayout.Vector3Field("Box Size", boxSize);
        color = EditorGUILayout.ColorField("Box Color", color);

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        for (int i = 0; i < positionHolder.localPositions.Count; i++)
        {
            // Convertir la posición local a posición mundial para mostrar y manipular el handle
            Vector3 worldPosition = positionHolder.transform.TransformPoint(positionHolder.localPositions[i]);

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(positionHolder, "Move Position Handle");
                // Convertir la posición manipulada de vuelta a local
                positionHolder.localPositions[i] = positionHolder.transform.InverseTransformPoint(newWorldPosition);
                EditorUtility.SetDirty(positionHolder);
            }

            // Dibujar la caja en la posición mundial
            if (drawDebugBox)
            {
                Handles.color = color;
                Handles.DrawWireCube(worldPosition, boxSize);
            }
        }
    }
}