using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, StateBase<EState>> States = new Dictionary<EState, StateBase<EState>>();
    protected StateBase<EState> stateActive;

    protected bool isTransitioningState = false;

    private async UniTask EnterInitialState(StateBase<EState> _state)
    {
        stateActive = _state;
        await stateActive.EnterState();
    }

    public void AddState<T>(EState stateKey, StateBase<EState> state, T mainClasRef) where T : class
    {
        state.InitState(mainClasRef);
        States.Add(stateKey, state);
    }

    // Update is called once per frame
    void Update()
    {
        if (stateActive == null) { return; }
        
        EState nextStateKey = stateActive.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(stateActive.StateKey))
        {
            stateActive.UpdateState();
        }
        else if(!isTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public async void TransitionToState(EState _newState)
    {
        isTransitioningState = true;
    
        await stateActive.ExitState();
        stateActive = States[_newState];
        await stateActive.EnterState();

        isTransitioningState = false;
    }
}
