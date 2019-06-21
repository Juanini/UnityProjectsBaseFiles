using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;
using GameEventSystem;
using System.IO;

using Sirenix.OdinInspector;

public class ModalPopup : MonoBehaviour {

    public static ModalPopup Instance;

    [FoldoutGroup("Loading")] public GameObject loadingMenu;
    [FoldoutGroup("Loading")] public GameObject loadingImag;
    [FoldoutGroup("Loading")] public float loadingSpeed;
    [FoldoutGroup("Loading")] public TextMeshProUGUI loadingText;
    [FoldoutGroup("Loading")] public float loadingMinWaitTime = 3;
    private UnityAction loadingCompleteCallback;

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

    public void ShowLoading(string text = "Loading", UnityAction callback = null)
    {
        loadingMenu.SetActive(true);

        if(callback != null)
        {
            loadingCompleteCallback = callback;
            Invoke("OnLoadingMinWaitComplete", loadingMinWaitTime);
        }
    }

    private void OnLoadingMinWaitComplete()
    {
        loadingCompleteCallback.Invoke();
    }
}
