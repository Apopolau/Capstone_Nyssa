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
        Debug.Log("******take Lightning Damage");
        if (player.enemySeen)
        {
           // player.LightningAttack();


        }





        player.isAttacking = false;
        Debug.Log("******lightning attack complete");


    }
    public override void ExitState(BaseStateMachine stateMachine)
    {

    }

}
