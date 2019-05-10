using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;
using GameEventSystem;
using Unity.VectorGraphics;
using System.IO;

using Sirenix.OdinInspector;

public class ModalPopup : MonoBehaviour {

    public static ModalPopup Instance;

    [FoldoutGroup("Loading")] public GameObject loadingMenu;
    [FoldoutGroup("Loading")] public GameObject loadingImag;
    [FoldoutGroup("Loading")] public float loadingSpeed;
    [FoldoutGroup("Loading")] public TextMeshProUGUI loadingText;

    private const string LOADING_JUST_A_MOMENT_TEXT = "Just a moment";

    ////=====================================================================================================================================
    //// Main

    void Awake()
    {
        Instance = this;       
        loadingMenu.SetActive(false);
    }

    ////=====================================================================================================================================
    //// Modal Popup

    public void ClosePanel()
    {
        loadingMenu.SetActive(false);
    }

    ////=====================================================================================================================================
    //// Loading

    public void ShowLoading(string text = LOADING_JUST_A_MOMENT_TEXT)
    {
        loadingText.text = text;
        loadingMenu.SetActive(true);
    }
}
