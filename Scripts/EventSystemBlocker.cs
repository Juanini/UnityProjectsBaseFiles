using System;
using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemBlocker : MonoBehaviour
{
    private EventSystem eventSystem;
    
    void Start()
    {
        eventSystem = GetComponent<EventSystem>();
        
        GameEventManager.StartListening(GameEvents.BLOCK_INPUT, OnBlockInput);
        GameEventManager.StartListening(GameEvents.RELEASE_INPUT, OnReleaseInput);
    }

    private void OnDestroy()
    {
        GameEventManager.StopListening(GameEvents.BLOCK_INPUT, OnBlockInput);
        GameEventManager.StopListening(GameEvents.RELEASE_INPUT, OnReleaseInput);
    }

    private void OnReleaseInput(Hashtable arg0)
    {
        eventSystem.enabled = true;
    }

    private void OnBlockInput(Hashtable arg0)
    {
        eventSystem.enabled = false;
    }
}
