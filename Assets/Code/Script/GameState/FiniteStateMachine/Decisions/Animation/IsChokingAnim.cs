using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/Animation/IsChoking")]
public class IsChoking : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<PlasticBagMonsterAnimator>().GetAnimationFlag("choke"))
        {
            return true;
        }
        return false;
    }
}
