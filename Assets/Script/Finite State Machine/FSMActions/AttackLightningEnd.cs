
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackLightningStrikeEnds")]
public class AttackLightningStrikeEnds : FSMAction
{
    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
     


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        if (player.enemySeen)
        {
            //player.LightningAttack();


        }

        player.isAttacking = false;
    }
}
