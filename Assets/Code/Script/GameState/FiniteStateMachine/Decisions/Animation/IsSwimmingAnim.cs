using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsSwimmingAnim")]
public class IsSwimming : Decision
{

    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<DuckAnimator>() != null)
        {
            if (stateMachine.GetComponent<DuckAnimator>().GetAnimationFlag("swim"))
            {
                return true;
            }
            return false;
        }
        return false;

    }
}
