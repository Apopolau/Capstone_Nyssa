using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsBuildingAnim")]
public class IsBuildingAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>().GetAnimationFlag("build"))
        {
            return true;
        }
        return false;
    }
}
