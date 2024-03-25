using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : ScriptableObject
{
    [SerializeField] protected bool isRemainInState;

    public virtual void EnterState(BaseStateMachine machine) { }

    public virtual void Execute(BaseStateMachine machine) { }

    public virtual void ExitState(BaseStateMachine machine) { }
    
    public virtual bool GetIsRemainInState() { return isRemainInState; }
}
