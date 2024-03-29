using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/AttackAnimationAction")]
public class AttackAnimationAction : FSMAction
{
    CelestialPlayer player;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {


        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP && stateMachine.GetComponent<CelestialPlayer>().canColdSnap ==true)
        {
           stateMachine.GetComponent<CelestialPlayer>().canColdSnap = false;
            Debug.Log("ColdSnapActivated");
            if (!player.canColdSnap)
            {
                player.StartCoroutine(player.animateColdSnap());
                // player.StartCoroutine(player.ResetColdSnap());
                player.ResetColdSnap();
               
            }
            Debug.Log("coldsnap stopped");


        }
        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        {
            Debug.Log("LightningActivated");


            if (!player.canLightningStrike)
            {
                player.StartCoroutine(player.animateLightningStrike());
                player.StartCoroutine(player.ResetLightningStrike());
            }
            Debug.Log("lightning  stopped");
     

        }
        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC)
        {
            Debug.Log("BasicActivated");


            if (!player.canBasicAttack)
            {
                player.StartCoroutine(player.animateBasicAttack());
                player.StartCoroutine(player.ResetBasic());

            }
            Debug.Log("basic stopped");
         

        }


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

}
