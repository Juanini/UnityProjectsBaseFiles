using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState _key)
    {
        StateKey = _key;
    }
    
    public EState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    
    public abstract void OnTriggerEnter(Collider _collider);
    public abstract void OnTriggerStay(Collider _collider);
    public abstract void OnTriggerExit(Collider _collider);
    
}
