using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMAction : ScriptableObject
{
    public abstract void EnterState(BaseStateMachine stateMachine);

    public abstract void Execute(BaseStateMachine stateMachine);

    public abstract void ExitState(BaseStateMachine stateMachine);
}
