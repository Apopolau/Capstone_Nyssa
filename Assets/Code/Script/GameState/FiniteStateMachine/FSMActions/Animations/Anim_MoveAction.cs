using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Move")]
public class Anim_MoveAction : FSMAction
{
    OurAnimator animatorScript;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        
        animatorScript.PlayAnimation("move");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
