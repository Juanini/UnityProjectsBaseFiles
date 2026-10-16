using DG.Tweening;
using HannieEcho.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPopupView : UIView
{
    public CanvasGroup container;
    public TextMeshProUGUI textDialog;
    public GameObject titleContainer;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public float infoSpace = 105f;
    public Button yesButton;
    public Button noButton;
    public Button cancelButton;

    private float fullHeight = -1f;

    public override void OnViewCreated()
    {
        base.OnViewCreated();
        
    }

    public void Init(
        string text,
        UnityAction yesEvent    = null, 
        UnityAction noEvent     = null, 
        UnityAction cancelEvent = null,
        string info             = null,
        string title            = null)
    {
        textDialog.text = text;

        var hasTitle = !string.IsNullOrEmpty(title);
        titleContainer.SetActive(hasTitle);
        if (hasTitle) titleText.text = title;

        var hasInfo = !string.IsNullOrEmpty(info);
        infoText.gameObject.SetActive(hasInfo);
        if (hasInfo) infoText.text = "<sprite=10> " + info;

        var panelRect = (RectTransform)container.transform;
        if (fullHeight < 0) fullHeight = panelRect.sizeDelta.y;
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, hasInfo ? fullHeight : fullHeight - infoSpace);

        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);
        
        if(yesEvent != null)
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener (yesEvent);

            yesButton.onClick.AddListener (HidePanel);
            yesButton.onClick.AddListener (Audio.PlayEnter);
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
        
        ShowAnimated();
    }

    private void ShowAnimated()
    {
        var animTime = 0.25f;
        var transform1 = container.transform.position;
        var originPos = transform1;

        container.alpha = 0;

        container.transform.position = new Vector3(transform1.x, transform1.y - 3, transform1.z);
        container.transform.DOMove(originPos, animTime);
        container.DOFade(1, animTime);
    }
    
    public void ClosePanel()
    {
        Audio.Ins.PlayOneShot(Audio.Ins.closeWindow);
        HidePanel();
    }

    private void HidePanel()
    {
        UI.Ins.uiNavigation.HideNavLastView();
    }
    
    private void TriggerButtonSound()
    {
		
    }
}