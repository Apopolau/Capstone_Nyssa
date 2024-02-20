using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/AttackColdSnapAction")]
public class AttackColdSnapAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();

        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP)
        {
            Debug.Log("ColdSnapActivated");

          
           
            player.StartCoroutine(player.animateColdSnap());
            player.StartCoroutine(player.ResetColdSnap());
           
            Debug.Log("coldsnap stopped");
            stateMachine.GetComponent<CelestialPlayer>().canColdSnap = false;

        }



    }

}
