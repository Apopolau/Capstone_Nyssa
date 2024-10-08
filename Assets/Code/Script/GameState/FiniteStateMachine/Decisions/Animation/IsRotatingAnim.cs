using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsRotatingAnim")]
public class IsRotatingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<EarthPlayerAnimator>().GetAnimationFlag("rotate"))
        {
            return true;
        }
        return false;
    }
}
