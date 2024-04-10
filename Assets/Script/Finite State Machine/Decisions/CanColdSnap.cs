using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "FSM/Decisions/canColdSnap")]
public class CanColdSnap : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
      
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();


        if (stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap && stateMachine.GetComponent<PowerBehaviour>().ColdSnapStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().canColdSnap && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP)
        {
            if (stateMachine.GetComponent<CelestialPlayer>().energy.current > -(attack.ColdSnapStats.energyDrain))
            {
                stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
                return true;
            }
            else
            {
                stateMachine.GetComponent<CelestialPlayer>().NotEnoughEnergy();
                stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap = false;
                return false;
            }
        }

        stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap = false;
        return false;
    }
}