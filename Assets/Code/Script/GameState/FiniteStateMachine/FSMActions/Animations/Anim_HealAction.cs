using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Animation/Heal")]
public class Anim_HealAction : FSMAction
{
    OurAnimator animatorScript;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        animatorScript = stateMachine.GetComponent<OurAnimator>();
        
        animatorScript.PlayAnimation("castHeal");
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
