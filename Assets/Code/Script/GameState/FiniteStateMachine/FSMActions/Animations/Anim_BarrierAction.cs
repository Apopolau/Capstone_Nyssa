using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Barrier")]
public class Anim_BarrierAction : FSMAction
{
    OurAnimator animatorScript;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        
        animatorScript.PlayAnimation("castBarrier");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();

        animatorScript.SetAnimationFlag("castBarrier", false);
        animatorScript.GetComponent<EarthPlayer>().BarrierWrapUp();
    }
}
