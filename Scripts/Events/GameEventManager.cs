using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/**
 * Implementation of a messaging/notification system. 
 * Allows subscription to events without referencing their definitions.
 * Using UnityEvents and a Hashtable for loosely typed params with event.
 * 
 * Usage: 
 * // Add Listener for Event
 * EventManager.StartListening ("MY_EVENT", MyEventHandlerMethodName);
 * 
 * // Trigger Event:
 * EventManager.TriggerEvent ("MY_EVENT", new Hashtable(){{"MY_EVENT_KEY", "valueOfAnyType"}});
 * 
 * // Pass null instead of a Hashtable if no params
 * EventManager.TriggerEvent ("MY_EVENT", null);
 *     
 * // Handler
 * private void HandleTeleportEvent (Hashtable eventParams){
 *    if (eventParams.ContainsKey("MY_EVENT")){
 *        // DO SOMETHING
 *    }
 * }
 *
 **/
namespace GameEventSystem
{
    public class GameEvent : UnityEvent<Hashtable> { }

    public class GameEventManager : MonoBehaviour
    {
        private Dictionary<string, GameEvent> eventDictionary;

        private static GameEventManager eventManager;

        public static GameEventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(GameEventManager)) as GameEventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, GameEvent>();
            }
        }

        public static void StartListening(string eventName, UnityAction<Hashtable> listener)
        {
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction<Hashtable> listener)
        {
            if (eventManager == null) return;
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }
        
        public static void StartListening(string eventName)
        {
            StartListening(eventName, null);
        }

        public static void TriggerEvent(string eventName)
        {
            TriggerEvent(eventName, null);
        }

        public static void TriggerEvent(string eventName, 
                                        Hashtable eventParams = default(Hashtable))
        {
            // Trace.Log("GameEventManager - Trigger Event: " + eventName);
            
            GameEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(eventParams);
            }
        }
    }
}