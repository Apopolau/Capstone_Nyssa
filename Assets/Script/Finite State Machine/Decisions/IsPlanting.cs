using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsPlanting")]
public class IsPlanting : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        if (stateMachine.GetComponent<EarthPlayer>().GetInPlantSelection())
        {
            //Debug.Log("Planting");
            return true;
        }
        //Debug.Log("Not planting");
        return false;
    }
}
