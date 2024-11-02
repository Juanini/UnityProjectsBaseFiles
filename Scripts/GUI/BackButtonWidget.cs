using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackButtonWidget : MonoBehaviour
{
    public Button backButton;
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
        backButton.onClick.AddListener(OnBackClick);
    }

    private void OnBackClick()
    {
        if (onBackClickEvent != null && onBackClickEvent.GetPersistentEventCount() > 0)
        {
            onBackClickEvent.Invoke();
        }
        else
        {
            UI.Ins.uiNavigation.HideNavLastView();
        }
    }

    public void SetOnBackClick(UnityAction action)
    {
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(action);
    }
}
