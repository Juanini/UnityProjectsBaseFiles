using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class StateBase<EState> where EState : Enum
{
    public StateBase(EState _key)
    {
        StateKey = _key;
    }

    private object storedReference;
    
    public void InitState<T>(T _classReference) where T : class
    {
        storedReference = _classReference;
    }
    
    public T GetMainClassReference<T>() where T : class
    {
        return storedReference as T;
    }
    
    public EState StateKey { get; private set; }

    public abstract UniTask EnterState();
    public abstract UniTask ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
}
