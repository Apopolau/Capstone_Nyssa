using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/isRespawning")]
public class isRespawning : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {
            //Debug.Log("Player is respawning");
       
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {
            //Debug.Log("Player is doing well ");
            return false;
        }
        return false;
    }
}
