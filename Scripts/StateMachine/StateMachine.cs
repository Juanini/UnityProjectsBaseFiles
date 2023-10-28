using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, StateBase<EState>> States = new Dictionary<EState, StateBase<EState>>();
    protected StateBase<EState> current;

    protected bool isTransitioningState = false;
    
    // void Start()
    // {
    //     UniTask.ToCoroutine(EnterInitialState);
    // }
    
    private async UniTask EnterInitialState()
    {
        await current.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        EState nextStateKey = current.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(current.StateKey))
        {
            current.UpdateState();
        }
        else if(!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public async void TransitionToState(EState _newState)
    {
        isTransitioningState = true;
    
        await current.ExitState();
        current = States[_newState];
        await current.EnterState();

        isTransitioningState = false;
    }
}
