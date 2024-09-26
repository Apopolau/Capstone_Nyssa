using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsStaggered")]
public class IsStaggered : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<Player>().IsStaggered())
        {
            return true;
        }
        return false;
    }
}
