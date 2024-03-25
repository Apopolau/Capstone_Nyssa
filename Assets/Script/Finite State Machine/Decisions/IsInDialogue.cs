using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsInDialogue")]
public class IsInDialogue : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetInDialogue())
        {
            //Debug.Log("InDialogue");
            return true;
        }
        //Debug.Log("Not in dialogue");
        return false;
    }
}
