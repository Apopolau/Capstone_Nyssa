using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsHealingAnim")]
public class IsHealingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("castHeal"))
        {
            return true;
        }
        return false;
    }
}
