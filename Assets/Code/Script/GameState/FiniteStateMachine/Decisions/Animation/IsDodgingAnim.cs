using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsDodgingAnim")]
public class IsDodgingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<CelestialPlayerAnimator>().GetAnimationFlag("dodge"))
        {
            return true;
        }
        return false;
    }
}
