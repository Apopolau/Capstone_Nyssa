using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "FSM/Decisions/canColdSnap")]
public class CanColdSnap : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        Debug.Log("Can CS Falseeeeeeee");

        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();


        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<PowerBehaviour>().ColdSnapStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().canColdSnap)
        {
            Debug.Log("Can CS True");
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking || !attack.ColdSnapStats.isEnabled || !stateMachine.GetComponent<CelestialPlayer>().canColdSnap)
        {
            Debug.Log("Can CS False");
            stateMachine.GetComponent<CelestialPlayer>().powerInUse = CelestialPlayer.Power.NONE;
            //Debug.Log("ColdSnap OVer------");
            return false;
        }
        return false;
    }
}