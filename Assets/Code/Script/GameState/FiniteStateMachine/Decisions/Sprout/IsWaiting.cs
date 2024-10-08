using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsPanning")]
public class IsWaiting : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<EarthPlayer>().GetIsWaiting())
        {
            return true;
        }
        return false;
    }
}
