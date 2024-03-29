
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackColdSnapEnds")]
public class AttackColdSnapEnds : FSMAction
{
    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        //Debug.Log("******takeDamage");
        if(player.enemySeen)
        { 
    

        }
 
        


          
        player.isAttacking = false;
        //Debug.Log("******tattack complete");


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}
