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
    
    public EState StateKey { get; private set; }

    public abstract UniTask EnterState();
    public abstract UniTask ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
}
