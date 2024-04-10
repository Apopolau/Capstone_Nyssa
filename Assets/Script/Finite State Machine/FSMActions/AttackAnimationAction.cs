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
            if (!player.canColdSnap)
            {
                player.StartCoroutine(player.animateColdSnap());
             
                player.ResetColdSnap();
               
            }
        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike == true)
        {
            stateMachine.GetComponent<CelestialPlayer>().canLightningStrike = false;


            if (!player.canLightningStrike)
            {
                player.StartCoroutine(player.animateLightningStrike());
                player.ResetLightningStrike();
            }
        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC && stateMachine.GetComponent<CelestialPlayer>().canBasicAttack == true)
        {
            stateMachine.GetComponent<CelestialPlayer>().canBasicAttack = false;

            if (!player.canBasicAttack)
            {
                player.StartCoroutine(player.animateBasicAttack());
                player.ResetBasic();

            }

        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE &&  stateMachine.GetComponent<CelestialPlayer>().canMoonTide)
        {
            stateMachine.GetComponent<CelestialPlayer>().canMoonTide = false;

            if (!player.canMoonTide)
            {
                player.StartCoroutine(player.animateMoonTide());
                player.ResetMoonTide();

            }
        }

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
       
    }

}
