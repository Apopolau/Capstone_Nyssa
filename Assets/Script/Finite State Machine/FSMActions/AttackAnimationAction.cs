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


        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP)
        {
            Debug.Log("ColdSnapActivated");

            player.StartCoroutine(player.animateColdSnap());
            player.StartCoroutine(player.ResetColdSnap());

            Debug.Log("coldsnap stopped");
            stateMachine.GetComponent<CelestialPlayer>().canColdSnap = false;

        }
        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        {
            Debug.Log("LightningActivated");



            player.StartCoroutine(player.animateLightningStrike());
            player.StartCoroutine(player.ResetLightningStrike());

            Debug.Log("coldsnap stopped");
            stateMachine.GetComponent<CelestialPlayer>().canLightningStrike = false;

        }
        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC)
        {
            Debug.Log("BasicActivated");



            player.StartCoroutine(player.animateBasicAttack());
            player.StartCoroutine(player.ResetBasic());

            Debug.Log("basic stopped");
            stateMachine.GetComponent<CelestialPlayer>().canBasicAttack = false;

        }


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

}
