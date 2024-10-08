using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsMovingAnim")]
public class IsMovingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<Actor>() != null)
        {
            if (stateMachine.GetComponent<Actor>().GetAnimator().GetAnimationFlag("move"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
