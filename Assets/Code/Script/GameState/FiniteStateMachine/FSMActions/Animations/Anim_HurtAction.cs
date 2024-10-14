using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Hurt")]
public class Anim_HurtAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();

        animatorScript.PlayAnimation("hurt");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        animatorScript.SetAnimationFlag("hurt", false);

        if (animatorScript.GetComponent<Player>() != null)
        {
            animatorScript.GetComponent<Player>().SuspendActions(false);
        }
    }
}
