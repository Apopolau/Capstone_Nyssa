using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsDead")]
public class IsDead : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<Player>().IsDead())
        {
            //Debug.Log("Dying");
            return true;
        }
        //Debug.Log("Not dying");
        return false;
    }
}
