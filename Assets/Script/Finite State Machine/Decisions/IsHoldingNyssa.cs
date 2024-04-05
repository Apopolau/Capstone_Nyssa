using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsHoldingNyssa")]
public class IsHoldingNyssa : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetIsHoldingNyssa())
        {
            //Debug.Log("InDialogue");
            return true;
        }
        //Debug.Log("Not in dialogue");
        return false;
    }
}
