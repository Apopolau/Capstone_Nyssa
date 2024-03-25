using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsHealing")]
public class IsHealing : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetInHealSelection())
        {
            //Debug.Log("Healing");
            return true;
        }
        return false;
    }
}
