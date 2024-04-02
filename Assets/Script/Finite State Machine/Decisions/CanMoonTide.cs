using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/CanMoonTide")]
public class CanMoonTide : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        //Debug.Log("CanMT Falseeeeeeee");
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();
        //If you press the button
        //If MoonTide hasn't been set to false
        //If Moontide has been enabled (found during the level)
        //If the power that is in use is moontide

        if (stateMachine.GetComponent<CelestialPlayer>().buttonMoonTide && stateMachine.GetComponent<CelestialPlayer>().canMoonTide && stateMachine.GetComponent<PowerBehaviour>().MoonTideAttackStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE)
        {
            stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;

            //Debug.Log("Can MY True");
            return true;
        }
        stateMachine.GetComponent<CelestialPlayer>().buttonMoonTide = false;


        return false;
    }
}