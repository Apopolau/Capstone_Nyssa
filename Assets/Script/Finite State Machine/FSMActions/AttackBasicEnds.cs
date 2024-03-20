
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackBasicEnds")]
public class AttackBasicEnds : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();
        CelestialPlayerBasicAttackTrigger staff = stateMachine.GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();

        //Debug.Log("******takeDamage");
        if (player.enemySeen && staff.enemyHit)
        {
            player.Attack();


        }





        player.isAttacking = false;
        //Debug.Log("******tattack complete");


    }

}

