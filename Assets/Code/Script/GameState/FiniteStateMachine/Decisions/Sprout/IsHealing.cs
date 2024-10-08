using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsHealing")]
public class IsHealing : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<EarthPlayer>().GetInHealSelection())
        {
            return true;
        }
        return false;
    }
}
