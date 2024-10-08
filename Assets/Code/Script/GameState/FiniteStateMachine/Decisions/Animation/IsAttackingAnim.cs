using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/InAttackingAnim")]
public class IsAttackingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>() != null)
        {
            if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("attack"))
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
