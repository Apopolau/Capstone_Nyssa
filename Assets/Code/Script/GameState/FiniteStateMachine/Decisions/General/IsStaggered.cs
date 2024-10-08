using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsStaggered")]
public class IsStaggered : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if(stateMachine.GetComponent<Player>() != null)
        {
            if (stateMachine.GetComponent<Player>().IsStaggered())
            {
                return true;
            }
            return false;
        }
        if(stateMachine.GetComponent<Enemy>() != null)
        {
            if (stateMachine.GetComponent<Enemy>().GetIsStaggered())
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
