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
             
                player.ResetColdSnap();
               
            }
            Debug.Log("coldsnap stopped");


        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike == true)
        {
            stateMachine.GetComponent<CelestialPlayer>().canLightningStrike = false;
            Debug.Log("LightningActivated");


            if (!player.canLightningStrike)
            {
                player.StartCoroutine(player.animateLightningStrike());
                player.ResetLightningStrike();
            }
            Debug.Log("lightning  stopped");
     

        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC && stateMachine.GetComponent<CelestialPlayer>().canBasicAttack == true)
        {
            stateMachine.GetComponent<CelestialPlayer>().canBasicAttack = false;

            Debug.Log("BasicActivated");


            if (!player.canBasicAttack)
            {
                player.StartCoroutine(player.animateBasicAttack());
                player.ResetBasic();

            }
            Debug.Log("basic stopped");
         

        }

        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE &&  stateMachine.GetComponent<CelestialPlayer>().canMoonTide)
        {
            stateMachine.GetComponent<CelestialPlayer>().canMoonTide = false;

            Debug.Log("MoonTideActivated");


            if (!player.canMoonTide)
            {
                player.StartCoroutine(player.animateMoonTide());
                player.ResetMoonTide();

            }
            Debug.Log("MoonTide stopped");


        }

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
       
    }

}
