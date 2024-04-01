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
            stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
            Debug.Log("Can CS True");
            return true;
        }
      /*  else if (stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap && !attack.ColdSnapStats.isEnabled || stateMachine.GetComponent<CelestialPlayer>().buttonColdSnap && !stateMachine.GetComponent<CelestialPlayer>().canColdSnap)
        {
          
            return false;
        }*/
        return false;
    }
}