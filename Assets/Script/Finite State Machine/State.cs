using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/State")]
public sealed class State : BaseState
{
    public List<FSMAction> Action = new List<FSMAction>();
    public List<Transition> Transitions = new List<Transition>();

    public override void EnterState(BaseStateMachine machine)
    {
        foreach (var action in Action)
        {
            action.EnterState(machine);
        }
    }

    public override void Execute(BaseStateMachine machine)
    {
        foreach (var action in Action)
        {
            action.Execute(machine);
        }
            

        foreach (var transition in Transitions)
        {
            transition.Execute(machine);
        }
            
    }

    public override void ExitState(BaseStateMachine machine)
    {
        foreach (var action in Action)
        {
            action.ExitState(machine);
        }
    }
}
