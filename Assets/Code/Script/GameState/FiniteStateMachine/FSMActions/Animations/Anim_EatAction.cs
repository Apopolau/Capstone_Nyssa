using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Eat")]
public class Anim_EatAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (animatorScript == null)
        {
            animatorScript = stateMachine.GetComponent<OurAnimator>();
        }
        
        animatorScript.PlayAnimation("eat", 0.1f);
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
