using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    protected bool isTransitioningState = false;
    
    void Start()
    {
        CurrentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if(!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState _newState)
    {
        isTransitioningState = true;
        
        CurrentState.ExitState();
        CurrentState = States[_newState];
        CurrentState.EnterState();

        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        CurrentState.OnTriggerEnter(_other);
    }
    
    private void OnTriggerStay(Collider _other)
    {
        CurrentState.OnTriggerEnter(_other);
    }
    
    private void OnTriggerExit(Collider _other)
    {
        CurrentState.OnTriggerEnter(_other);
    }
}
