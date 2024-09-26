using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/isRespawning")]
public class isRespawning : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {
       
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {
            return false;
        }
        return false;
    }
}
