
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackBasicEnds")]
public class AttackBasicEnds : FSMAction
{
    CelestialPlayer player;
    CelestialPlayerBasicAttackTrigger staff;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
       staff = stateMachine.GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();
    }


    public override void Execute(BaseStateMachine stateMachine)
    {

        //Debug.Log("******takeDamage");
        if (player.enemySeen && staff.enemyHit)
        {
           // player.BasicAttack();


        }





        player.isAttacking = false;
        //Debug.Log("******tattack complete");


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}

