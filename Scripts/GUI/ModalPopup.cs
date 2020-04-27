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

    [BoxGroup("Popup")] public GameObject askMenuContainer;
    [BoxGroup("Popup")] public GameObject Achis;
    [BoxGroup("Popup")] public Button yesButton;
    [BoxGroup("Popup")] public TextMeshProUGUI yesButtonText;
    [BoxGroup("Popup")] public Button noButton;
    [BoxGroup("Popup")] public TextMeshProUGUI noButtonText;
    [BoxGroup("Popup")] public Button cancelButton;
    [BoxGroup("Popup")] public TextMeshProUGUI dialogText;

    ////=====================================================================================================================================
    //// Main

    void Awake()
    {
        Instance = this;       
    }

    ////=====================================================================================================================================
    //// Modal Popup

    public void ClosePanel()
    {
        GameEventManager.TriggerEvent(GameEvents.COLLIDER_BLOCK_OFF);

        Achis.gameObject.SetActive(false);
        askMenuContainer.SetActive(false);
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

    // * =====================================================================================================================================
    // * Popup

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void CreatePopup (
		string question,
        Sprite image            = null, 
		UnityAction yesEvent    = null, 
		UnityAction noEvent     = null, 
		UnityAction cancelEvent = null,
        UnityAction exitEvent   = null,
        bool closeLoading       = true,
        string yesBttnString    = "YES", 
        string noBttnString     = "NO",
        bool isCritical         = false,
        string imageURL         = null, 
        string userName         = "",
        bool showPicture        = false, 
        bool showExit           = false,
        bool reverseButtonsOrder     = false) 
	{
        GameEventManager.TriggerEvent(GameEvents.COLLIDER_BLOCK_ON);

        Trace.Log("Achis" + Achis == null ? "0" : "1");
        Trace.Log("yesButton" + yesButton == null ? "0" : "1");
        Trace.Log("noButton" + noButton == null ? "0" : "1");
        Trace.Log("cancelButton" + cancelButton == null ? "0" : "1");

        Achis.gameObject.SetActive(true);

        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);

        yesButtonText.text = yesBttnString;
        noButtonText.text = noBttnString;

        if(yesEvent != null)
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener (yesEvent);

            yesButton.onClick.AddListener (ClosePanel);
            yesButton.onClick.AddListener (TriggerButtonSound);
            yesButton.gameObject.SetActive (true);
        }
        
        if(noEvent != null)
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener (noEvent);
            noButton.onClick.AddListener (ClosePanel);
            noButton.onClick.AddListener (TriggerButtonSound);

            noButton.gameObject.SetActive (true);
        }

        if (reverseButtonsOrder)
        {
            yesButton.gameObject.transform.SetAsLastSibling();
        }

        if (cancelEvent != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener (cancelEvent);
            cancelButton.onClick.AddListener (ClosePanel);
            cancelButton.onClick.AddListener (TriggerButtonSound);

            cancelButton.gameObject.SetActive (true);
        }

        this.dialogText.text = question;

    }

    private void TriggerButtonSound()
	{
		AudioManager.Main.PlayNewSound(AudioConstants.AUDIO_GUI_CLICK);
    }
}
