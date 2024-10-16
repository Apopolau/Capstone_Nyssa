using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Transition")]
public sealed class Transition : ScriptableObject
{
    public Decision Decision;
    public BaseState TrueState;
    public BaseState FalseState;
    public BaseState remainInState;

    public bool Execute(BaseStateMachine stateMachine)
    {
        if ((Decision.Decide(stateMachine)) && (TrueState != remainInState) && (TrueState != null) && (!TrueState.GetIsRemainInState()))
        {
            stateMachine.CurrentState.ExitState(stateMachine);
            stateMachine.CurrentState = TrueState;
            stateMachine.CurrentState.EnterState(stateMachine);
            return true;
        }
        else if ((!Decision.Decide(stateMachine) && (FalseState != remainInState) && (FalseState != null) && (!FalseState.GetIsRemainInState())))
        {
            stateMachine.CurrentState.ExitState(stateMachine);
            stateMachine.CurrentState = FalseState;
            stateMachine.CurrentState.EnterState(stateMachine);
            return true;
        }
        return false;
    }
}
