using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/CanMoonTide")]
public class CanMoonTide : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();
        //If you press the button
        //If MoonTide hasn't been set to false
        //If Moontide has been enabled (found during the level)
        //If the power that is in use is moontide

        //if (stateMachine.GetComponent<CelestialPlayer>().buttonMoonTide && stateMachine.GetComponent<CelestialPlayer>().canMoonTide && stateMachine.GetComponent<PowerBehaviour>().MoonTideAttackStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE)
        if (stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.MOONTIDE)
        {
            /*
            //if (stateMachine.GetComponent<CelestialPlayer>().energy.current > -(attack.MoonTideAttackStats.energyDrain))
            if (stateMachine.GetComponent<CelestialPlayer>().CheckIfCastable(attack.LightningStats, false))
            {
                stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
                return true;
            }
            else
            {
                //stateMachine.GetComponent<CelestialPlayer>().NotEnoughEnergy();
                stateMachine.GetComponent<CelestialPlayer>().buttonMoonTide = false;
                return false;
            }
            */
            stateMachine.GetComponent<CelestialPlayer>().SetIsAttacking(true);
            return true;
        }
        //stateMachine.GetComponent<CelestialPlayer>().buttonMoonTide = false;


        return false;
    }
}