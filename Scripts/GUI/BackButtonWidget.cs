using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackButtonWidget : MonoBehaviour
{
    public ButtonCustom backButton;
    private bool isInitialized;
    [SerializeField] private bool autoInit = false;
    
    private UnityEvent onBackClickEvent;
    
    private void Start()
    {
        if (autoInit)
        {
            Init();
        }
    }

    private void Init()
    {
        if (isInitialized) { return; }
        
        isInitialized = true;
        backButton.SetAction(OnBackClick);
    }

    private void OnBackClick()
    {
        if (onBackClickEvent != null && onBackClickEvent.GetPersistentEventCount() > 0)
        {
            Audio.PlayUIClick();
            onBackClickEvent.Invoke();
        }
        else
        {
            Audio.PlayUIClick();
            UI.Ins.uiNavigation.HideNavLastView();
        }
    }

    public void SetOnBackClick(UnityAction action)
    {
        backButton.button.onClick.RemoveAllListeners();
        backButton.SetAction(action);
    }
}
