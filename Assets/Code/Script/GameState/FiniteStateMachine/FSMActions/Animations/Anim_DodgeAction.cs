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
        
        animatorScript.PlayAnimation("dodge", 0.1f);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript.GetComponent<CelestialPlayer>().StopDodgeMovement();
        animatorScript.GetComponent<CelestialPlayer>().EndIFrames();
        animatorScript.SetAnimationFlag("dodge", false);
    }
}
