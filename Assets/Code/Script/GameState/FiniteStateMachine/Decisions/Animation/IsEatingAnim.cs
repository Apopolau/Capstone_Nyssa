using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsEatingAnim")]
public class IsEatingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<AnimalAnimator>().GetAnimationFlag("eat"))
        {
            return true;
        }
        return false;
    }
}
