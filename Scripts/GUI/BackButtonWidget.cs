using UnityEngine;
using UnityEngine.UI;

public class BackButtonWidget : MonoBehaviour
{
    public Button backButton;
    private bool isInitialized;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (isInitialized) { return; }
        
        isInitialized = true;
        backButton.onClick.AddListener(OnBackClick);
    }

    private void OnBackClick()
    {
        UI.Ins.uiNavigation.HideNavLastView();
    }
}
