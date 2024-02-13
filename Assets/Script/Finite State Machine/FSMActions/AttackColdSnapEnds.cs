
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackColdSnapEnds")]
public class AttackColdSnapEnds : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();
    

        Debug.Log("******takeDamage");
        if(player.enemySeen)
        {
            player.Attack();


        }
 
        


          
        player.isAttacking = false;
        Debug.Log("******tattack complete");


    }

}
