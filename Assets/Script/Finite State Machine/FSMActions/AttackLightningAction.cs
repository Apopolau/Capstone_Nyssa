using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackLightningAction")]
public class AttackLightningAction: FSMAction
{
   
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();


        Debug.Log("******take Lightning Damage");
        if (player.enemySeen)
        {
            player.Attack();


        }





        player.isAttacking = false;
        Debug.Log("******lightning attack complete");


    }



}
