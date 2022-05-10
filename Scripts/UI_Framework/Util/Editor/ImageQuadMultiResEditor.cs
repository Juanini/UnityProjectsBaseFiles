using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace HannieEcho.UI
{
    [CustomEditor(typeof(ImageQuadMultiRes))]
    public class ImageQuadMultiResEditor : GraphicEditor
    {
        ImageQuadMultiRes m_Target => target as ImageQuadMultiRes;

        SerializedProperty m_QuadResSF;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_QuadResSF = serializedObject.FindProperty("m_QuadResAtInit");
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_QuadResSF);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Reset Quad res"))
                m_Target.ResetQuadRes();
        }
    }
}