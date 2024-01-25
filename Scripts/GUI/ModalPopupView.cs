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
    public Button yesButton;
    public Button noButton;
    public Button cancelButton;
    
    public override void OnViewCreated()
    {
        base.OnViewCreated();
        
    }

    public void Init(
        string text,
        UnityAction yesEvent    = null, 
        UnityAction noEvent     = null, 
        UnityAction cancelEvent = null)
    {
        textDialog.text = text;
        
        yesButton.gameObject.SetActive (false);
        noButton.gameObject.SetActive (false);
        cancelButton.gameObject.SetActive (false);
        
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
        
        ShowAnimated();
    }

    private void ShowAnimated()
    {
        var animTime = 0.25f;
        var transform1 = container.transform.position;
        var originPos = transform1;

        container.alpha = 0;

        container.transform.position = new Vector3(transform1.x, transform1.y - 50, transform1.z);
        container.transform.DOMove(originPos, animTime);
        container.DOFade(1, animTime);
    }
    
    public void ClosePanel()
    {
        // UI.Ins.nav.HideNavLastView();
    }
    
    private void TriggerButtonSound()
    {
		
    }
}