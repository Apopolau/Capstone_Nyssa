using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsShieldingAnim")]
public class IsShieldingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("castBarrier"))
        {
            return true;
        }
        return false;
    }
}
