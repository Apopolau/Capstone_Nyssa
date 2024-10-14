using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Cast")]
public class Anim_CastAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        
        animatorScript.PlayAnimation("cast");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript.SetAnimationFlag("cast", false);
        animatorScript.GetComponent<CelestialPlayer>().SuspendActions(false);
        animatorScript.GetComponent<CelestialPlayer>().SetIsAttacking(false);
    }
}
