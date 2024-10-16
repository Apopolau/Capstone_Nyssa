using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Celeste/AttackLightningAction")]
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

        player.SetIsAttacking(false);

    }
    public override void ExitState(BaseStateMachine stateMachine)
    {

    }

}
