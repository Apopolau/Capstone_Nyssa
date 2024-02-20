using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "FSM/Decisions/canColdSnap")]
public class CanColdSnap : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking)
        {
            Debug.Log("currentdoing attack");
           
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking)
        {
            //Debug.Log("ColdSnap OVer------");
            return false;
        }
        return false;
    }
}