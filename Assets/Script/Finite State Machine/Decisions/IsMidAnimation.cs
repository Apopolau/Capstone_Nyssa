using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsMidAnimation")]
public class IsMidAnimation : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetMidAnimation())
        {
            //Debug.Log("Mid-Animation");
            return true;
        }
        return false;
    }
}
