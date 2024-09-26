using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsRemoving")]
public class IsRemoving : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<EarthPlayer>().GetInRemovalSelection())
        {
            return true;
        }
        return false;
    }
}
