using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Attack")]
public class Anim_AttackAction : FSMAction
{
    OurAnimator animatorScript;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (animatorScript == null)
        {
            animatorScript = stateMachine.GetComponent<OurAnimator>();
        }
        animatorScript.PlayAnimation(("attack"), 0.1f);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
