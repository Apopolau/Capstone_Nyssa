using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Attack")]
public class Anim_AttackAction : FSMAction
{
    OurAnimator animatorScript;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        
        animatorScript.PlayAnimation("attack");
        if (animatorScript.GetComponent<CelestialPlayer>() != null)
        {
            animatorScript.GetComponent<CelestialPlayer>().SuspendActions(true);
        }
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    //Shut off any animation events that may not have played by the time this animation gets stopped
    public override void ExitState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();

        if (animatorScript.GetComponent<CelestialPlayer>() != null)
        {
            animatorScript.GetComponent<CelestialPlayer>().AttackCollisionOff();
            animatorScript.SetAnimationFlag("attack", false);
            animatorScript.GetComponent<CelestialPlayer>().SuspendActions(false);
            animatorScript.GetComponent<CelestialPlayer>().SetIsAttacking(false);
        }
        if(animatorScript.GetComponent<KidnappingEnemy>() != null)
        {
            animatorScript.GetComponent<KidnappingEnemy>().AttackCollisionOff();
            animatorScript.SetAnimationFlag("attack", false);
        }
    }
}
