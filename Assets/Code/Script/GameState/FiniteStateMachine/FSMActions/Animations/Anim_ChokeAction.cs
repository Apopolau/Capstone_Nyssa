using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Choke")]
public class Anim_ChokeAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (animatorScript == null)
        {
            animatorScript = stateMachine.GetComponent<OurAnimator>();
        }
        
        animatorScript.PlayAnimation("choke");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
