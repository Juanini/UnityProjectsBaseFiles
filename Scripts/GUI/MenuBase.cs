using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using GameEventSystem;

// using LifeIsTheGame;

public class MenuBase : MonoBehaviour {

    public void onEnterScreen()
    {
        // Do basic logic for opening
        preActive();
        InitScreen();
        
        gameObject.SetActive(true);
        postActive();
    }

    public virtual void onExitScreen()
    {
        // Do basic logic for closing
        preDeactive();
        gameObject.SetActive(false);
    }

	public void ShowMenu(int menu, bool closePrivous = true)
    {
        Hashtable hashParams = new Hashtable();
        hashParams.Add(GameEventParam.SCREEN_TO_OPEN, menu);

        GameEventManager.TriggerEvent(GameEvents.E_OPEN_SCREEN, hashParams);
    }

    public void SetCloseButton(Button closeButton)
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }
    
    public void OnCloseButtonClick()
    {
        onExitScreen();
    }

	public virtual void InitScreen () {}
    public virtual void preActive  () {}
    public virtual void postActive () {}
    public virtual void preDeactive() {}
}
