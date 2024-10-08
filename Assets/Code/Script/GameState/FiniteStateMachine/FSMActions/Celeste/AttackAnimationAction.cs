using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Architecture/FSM/Actions/Celeste/AttackAnimationAction")]
public class AttackAnimationAction : FSMAction
{
    CelestialPlayer player;
    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {


        if (stateMachine.GetComponent<CelestialPlayer>().GetIsAttacking() && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP && stateMachine.GetComponent<CelestialPlayer>().GetCanColdSnap())
        {
           stateMachine.GetComponent<CelestialPlayer>().SetCanColdSnap(false);
            if (!player.GetCanColdSnap())
            {
                
                player.StartCoroutine(player.animateColdSnap());
               
                player.ResetColdSnap();
               
            }
        }

        if (stateMachine.GetComponent<CelestialPlayer>().GetIsAttacking() && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE && stateMachine.GetComponent<CelestialPlayer>().GetCanLightningStrike())
        {
            stateMachine.GetComponent<CelestialPlayer>().SetCanLightningStrike(false);


            if (!player.GetCanLightningStrike())
            {
                player.StartCoroutine(player.animateLightningStrike());
                player.ResetLightningStrike();
            }
        }

        if (stateMachine.GetComponent<CelestialPlayer>().GetIsAttacking() && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC && stateMachine.GetComponent<CelestialPlayer>().GetCanBasicAttack())
        {
            stateMachine.GetComponent<CelestialPlayer>().SetCanBasicAttack(false);

            if (!player.GetCanBasicAttack())
            {
                player.StartCoroutine(player.animateBasicAttack());
                player.ResetBasic();

            }

        }

        if (stateMachine.GetComponent<CelestialPlayer>().GetIsAttacking() && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE &&  stateMachine.GetComponent<CelestialPlayer>().GetCanMoonTide())
        {
            stateMachine.GetComponent<CelestialPlayer>().SetCanMoonTide(false);

            if (!player.GetCanMoonTide())
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
