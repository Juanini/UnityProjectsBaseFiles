using System;
using System.Collections.Generic;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonCustom : MonoBehaviour
{
    [BoxGroup("Elements")] public TextMeshProUGUI buttonTextLabel;

    [BoxGroup("Properties")] public string buttonTextStr;
    public Button button;
    
    [BoxGroup("TUTORIAL")] public bool isTutorialButton;

    private UnityAction action;
    
    void Awake()
    {
        button = GetComponent<Button>();
        SubscribeToTutoEvents();
    }

    public Button GetButton()
    {
        return !button ? GetComponent<Button>() : button;
    }

    public void SetAction(UnityAction _action)
    {
        Trace.Log("Button Custom - Action: ");
        
        action = _action;
        
        RemoveAllListeners();
        GetButton().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if(!CanBeClicked()) return;
        
        action.Invoke();
        ButtonSound();
        TutoClickAction();
    }

    private bool CanBeClicked()
    {
        if (Game.IsInputBlocked) return false;
        if (!TutorialManager.Ins.IsActive) return true;
        if (!isTutorialButton) return false;
        return TutorialManager.Ins.GetCurrentWaitingButtonID() == tutoIds;
    }

    public void SetText(string text)
    {
        buttonTextStr = text;
        buttonTextLabel.text = buttonTextStr;
    }

    public void SetInteractable(bool enabled)
    {
        GetButton().interactable = enabled;
    }

    public void ButtonSound()
    {
        Audio.PlayUIClick();
    }

    public void RemoveAllListeners()
    {
        GetButton().onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        UnsubscribeToTutoEvents();
    }

    // ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    #region TUTORIAL ELEMENTS

    [ShowIf("isTutorialButton")][BoxGroup("TUTORIAL")] 
    public string tutoIds;

    [ShowIf("isTutorialButton")] [BoxGroup("TUTORIAL")]
    public ScriptableEventString focusEvent;
    
    [ShowIf("isTutorialButton")] [BoxGroup("TUTORIAL")]
    public ScriptableEventString clickEvent;
    
    [ShowIf("isTutorialButton")] [BoxGroup("TUTORIAL")]
    public int tutorialCustomNumberId;
    
    private void OnTutoFocusEvent(string _string)
    {
        if(_string != tutoIds) return;
        TutorialHandPointer.Ins.FocusOnPosition(transform);
    }
    
    private void TutoClickAction()
    {
        if (!TutorialManager.Ins.IsActive) return;
        if(!isTutorialButton) return;
        
        clickEvent?.Raise(tutoIds);
        TutorialHandPointer.Ins.Hide();
        TutorialFocusUI.Ins.HideAnimated();
    }
    
    private void SubscribeToTutoEvents()
    {
        if(!isTutorialButton) return;
        focusEvent.OnRaised += OnTutoFocusEvent;
    }

    private void UnsubscribeToTutoEvents()
    {
        if(!isTutorialButton) return;
        focusEvent.OnRaised -= OnTutoFocusEvent;
    }

    public void SetsTutorialCustomNumberId(int _tutorialCustomNumberId)
    {
        tutorialCustomNumberId =  _tutorialCustomNumberId;
        tutoIds = tutoIds + _tutorialCustomNumberId;
    }

    #endregion
}
