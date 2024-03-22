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


        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().canColdSnap  && stateMachine.GetComponent<PowerBehaviour>().ColdSnapStats.isEnabled)
        {
            Debug.Log("currentdoing cold snap attack");
           
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking || !attack.ColdSnapStats.isEnabled)
        {
            //Debug.Log("ColdSnap OVer------");
            return false;
        }
        return false;
    }
}