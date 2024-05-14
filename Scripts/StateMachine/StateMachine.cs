using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, StateBase<EState>> States = new Dictionary<EState, StateBase<EState>>();
    protected StateBase<EState> stateActive;
    
    public delegate void OnStateTransition(EState newState);
    public event OnStateTransition StateTransitioned;

    protected bool isTransitioningState = false;

    public async UniTask EnterInitialState(StateBase<EState> _state)
    {
        stateActive = _state;
        await stateActive.EnterState();
    }

    public void AddState<T>(EState stateKey, StateBase<EState> state, T mainClasRef) where T : class
    {
        state.InitState(mainClasRef);
        States.Add(stateKey, state);
    }

    public T GetState<T>(EState _stateKey) where T : class
    {

        if (States.ContainsKey(_stateKey))
        {
            return States[_stateKey] as T;
        }
        return null;
    }

    public StateBase<EState> GetCurrentState()
    {
        return stateActive;
    }

    // Update is called once per frame
    public void Update()
    {
        if (stateActive == null) { return; }

        if (!isTransitioningState)
        {
            stateActive.UpdateState();
        }
    }

    public async void TransitionToState(EState _newState)
    {
        isTransitioningState = true;
    
        await stateActive.ExitState();
        stateActive = States[_newState];
        await stateActive.EnterState();

        isTransitioningState = false;
        
        StateTransitioned?.Invoke(_newState);
    }
}
