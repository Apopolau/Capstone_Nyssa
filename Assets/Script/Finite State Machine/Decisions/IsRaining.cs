using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/isRaining")]
public class isRaining : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().buttonRain)
        {
     
            return true;
        }
     
        return false;
    }
}
