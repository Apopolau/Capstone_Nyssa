using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsPanning")]
public class IsPanning : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<EarthPlayer>().GetIsPanning())
        {
            //Debug.Log("Panning");
            return true;
        }
        //Debug.Log("Not panning");
        return false;
    }
}
