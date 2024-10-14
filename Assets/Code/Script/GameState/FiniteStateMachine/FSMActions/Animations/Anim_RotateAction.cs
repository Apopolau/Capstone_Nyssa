using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Rotate")]
public class Anim_RotateAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (animatorScript == null)
        {
            animatorScript = stateMachine.GetComponent<OurAnimator>();
        }
        
        animatorScript.PlayAnimation("rotate");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
