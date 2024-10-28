using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class StateBase<EState> : MonoBehaviour where EState : Enum
{
    public StateBase(EState _key)
    {
        StateKey = _key;
    }

    protected object context;
    
    public void InitState<T>(T _classReference, EState _stateKey) where T : class
    {
        context = _classReference;
        StateKey = _stateKey;
    }
    
    public T GetContext<T>() where T : class
    {
        return context as T;
    }
    
    public EState StateKey { get; private set; }

    public abstract UniTask EnterState();
    public abstract UniTask ExitState();
    public abstract void UpdateState();
}
