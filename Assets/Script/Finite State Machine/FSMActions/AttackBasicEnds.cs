
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Attack Basic Ends")]
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

        if (player.enemySeen && staff.enemyHit)
        {
           // player.BasicAttack();


        }





        player.isAttacking = false;

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}

