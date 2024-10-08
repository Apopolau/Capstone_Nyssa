using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsPanickingAnim")]
public class IsPanicking : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<AnimalAnimator>().GetAnimationFlag("panic"))
        {
            return true;
        }
        return false;
    }
}
