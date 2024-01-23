#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneExplorer : EditorWindow
{
    public static List<string> scenePaths = new List<string>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Scene Explorer")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneExplorer>();
        window.titleContent = new GUIContent("SE");
        GetScenes();
        window.Show();
    }

    private void OnEnable() 
    {
        GetScenes();
    }

    private static void GetScenes()
    {
        scenePaths.Clear();
        string[] guids = AssetDatabase.FindAssets("t:scene", null);

        List<EditorBuildSettingsScene> editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();

        foreach (string guid in guids)
        {
            if(editorBuildSettingsScenes.Find((scene) => scene.path == AssetDatabase.GUIDToAssetPath(guid)) != null)
                scenePaths.Add(AssetDatabase.GUIDToAssetPath(guid));
        }
    }

    static SceneExplorer()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) => { GetScenes(); };
    }

    private static void ContextMenu()
    {
        Rect clickArea = EditorGUILayout.GetControlRect();

        Event current = Event.current;

        if(mouseOverWindow && current.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Refresh scens"), false, GetScenes);
            menu.ShowAsContext();

            current.Use();
        }
    }

    private void OnGUI() 
    {
        ContextMenu();
        GUILayout.BeginVertical();
        GUIStyle titleStile = new GUIStyle(EditorStyles.boldLabel);
        titleStile.alignment = TextAnchor.MiddleCenter;
        titleStile.fontSize = 24;

        if(GUILayout.Button("Scene explorer", titleStile, GUILayout.Height(30)))
        {
            GetScenes();
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        List<EditorBuildSettingsScene> editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();

        for (int i = 0; i < scenePaths.Count; i++)
        {
            GUILayout.BeginHorizontal();
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePaths[i]);
            var sceneItem = editorBuildSettingsScenes.Find((scene) => scene.path == scenePaths[i]);
            
            if(EditorSceneManager.GetActiveScene().path == scenePaths[i])
            {
                GUI.color = Color.green;
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.Width(30)))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive);
            }

            if (GUILayout.Button(sceneAsset.name))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scenePaths[i]);    
            }

            GUI.color = Color.white;
            
            // if(GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom"), GUILayout.Width(30)))
            // {
            //     EditorGUIUtility.PingObject(sceneAsset);
            // }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

    }
}
#endif