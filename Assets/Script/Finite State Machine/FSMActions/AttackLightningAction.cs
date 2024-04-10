using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackLightningAction")]
public class AttackLightningAction: FSMAction
{
    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        if (player.enemySeen)
        {

        }

        player.isAttacking = false;

    }
    public override void ExitState(BaseStateMachine stateMachine)
    {

    }

}
