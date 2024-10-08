using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsCarryingAnim")]
public class IsCarryingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("carry"))
        {
            return true;
        }
        return false;
    }
}
