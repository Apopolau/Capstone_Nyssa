using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsCastingCelesteAnim")]
public class IsCastingCelesteAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<CelestialPlayerAnimator>().GetAnimationFlag("cast"))
        {
            return true;
        }
        return false;
    }
}
