using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsDyingAnim")]
public class IsDyingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("die"))
        {
            return true;
        }
        return false;
    }
}
