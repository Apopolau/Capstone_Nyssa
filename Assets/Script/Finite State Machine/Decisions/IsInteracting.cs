using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsInteracting")]
public class IsInteracting : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetIsInteracting())
        {
            //Debug.Log("Interacting");
            return true;
        }
        //Debug.Log("Not interacting");
        return false;
    }
}
