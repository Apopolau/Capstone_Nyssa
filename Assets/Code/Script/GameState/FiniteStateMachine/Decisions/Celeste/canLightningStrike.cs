using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/CanLightningStrike")]
public class CanLightningStrike: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();

        //if (stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike && stateMachine.GetComponent<PowerBehaviour>().LightningStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        if (stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        {
            //Debug.Log("Attempting to cast Lightning Strike");
            //if (stateMachine.GetComponent<CelestialPlayer>().energy.current > -(attack.LightningStats.energyDrain))
            /*
            if (stateMachine.GetComponent<CelestialPlayer>().CheckIfCastable(attack.LightningStats, false))
            {
                stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
                return true;
            }
            else 
            {
                //stateMachine.GetComponent<CelestialPlayer>().NotEnoughEnergy();
                stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike = false;
                return false;
            }
            */
            stateMachine.GetComponent<CelestialPlayer>().SetIsAttacking(true);
            return true;

        }
        //stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike = false;
        return false;
    }
}