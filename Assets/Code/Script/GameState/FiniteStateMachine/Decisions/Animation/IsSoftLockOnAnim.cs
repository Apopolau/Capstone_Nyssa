using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsSoftLockOnAnim")]
public class IsSoftLockOnAnim : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<OurAnimator>() != null)
        {
            if (stateMachine.GetComponent<OurAnimator>().GetInSoftLock())
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
