using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/canColdSnap")]
public class CanColdSnap : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
      
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();


        //if (stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap && stateMachine.GetComponent<PowerBehaviour>().ColdSnapStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().canColdSnap && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP)
        if (stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.COLDSNAP)
        {
            //Debug.Log("Attempting to cast Cold Snap");
            /*
            //if (stateMachine.GetComponent<CelestialPlayer>().energy.current > -(attack.ColdSnapStats.energyDrain))
            if(stateMachine.GetComponent<CelestialPlayer>().CheckIfCastable(attack.ColdSnapStats, false))
            {
                stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
                return true;
            }
            else
            {
                Debug.Log("Couldn't cast Cold Snap");
                //stateMachine.GetComponent<CelestialPlayer>().NotEnoughEnergy();
                stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap = false;
                return false;
            }
            */
            stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
            return true;

        }

        stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap = false;
        return false;
    }
}