using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Dodge")]
public class Anim_DodgeAction : FSMAction
{
    CelestialPlayerAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (animatorScript == null)
        {
            animatorScript = stateMachine.GetComponent<CelestialPlayerAnimator>();
        }
        
        animatorScript.PlayAnimation("dodge");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript.GetComponent<CelestialPlayer>().StopDodgeMovement();
        animatorScript.GetComponent<CelestialPlayer>().EndIFrames();
        animatorScript.GetComponent<CelestialPlayer>().SuspendActions(false);
        animatorScript.SetAnimationFlag("dodge", false);
    }
}
