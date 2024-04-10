using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/CanLightningStrike")]
public class CanLightningStrike: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        //Debug.Log("Can LS Falseeeeeeee");
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();
 
        if (stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike && stateMachine.GetComponent<PowerBehaviour>().LightningStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        {
            if (stateMachine.GetComponent<CelestialPlayer>().energy.current > -(attack.LightningStats.energyDrain))
            {
                stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
                return true;
            }
            else 
            {
                stateMachine.GetComponent<CelestialPlayer>().NotEnoughEnergy();
                stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike = false;
                return false;
            }

        }
        stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike = false;
        return false;
    }
}