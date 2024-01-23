using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;
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
    [BoxGroup("Popup")] public GameObject popupMenu;
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

    public void ShowConfirmationPopup(string _dialog)
    {
        this.dialogText.text = _dialog;
        Achis.gameObject.SetActive(true);
        popupMenu.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener (ClosePanel);
        cancelButton.onClick.AddListener (TriggerButtonSound);

        cancelButton.gameObject.SetActive (true);
    }

    public void ShowNotificationPopup(string _dialog)
    {
        this.dialogText.text = _dialog;
        Achis.gameObject.SetActive(true);
        popupMenu.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);
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
        Achis.gameObject.SetActive(true);
        popupMenu.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);

        noButtonText.text = "No";
        yesButtonText.text = "Yes";

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

        if(yesEvent != null && noEvent != null && cancelEvent != null)
        {
            yesButtonText.text = "Ok";
            
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener (yesEvent);

            yesButton.onClick.AddListener (ClosePanel);
            yesButton.onClick.AddListener (TriggerButtonSound);
            yesButton.gameObject.SetActive (true);
        }

        this.dialogText.text = question;

    }

    private void TriggerButtonSound()
	{
		
    }
}
