using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsStaggered")]
public class IsStaggered : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<Player>().IsStaggered())
        {
            //Debug.Log("Staggered");
            return true;
        }
        //Debug.Log("Not staggered");
        return false;
    }
}
