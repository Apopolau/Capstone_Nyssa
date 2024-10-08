using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/IsDead")]
public class IsDead : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>();
        if(stateMachine.GetComponent<Player>() != null)
        {
            if (stateMachine.GetComponent<Player>().IsDead())
            {
                return true;
            }
            return false;
        }
        if(stateMachine.GetComponent<Enemy>() != null)
        {
            if (stateMachine.GetComponent<Enemy>().GetIsDying())
            {
                return true;
            }
            return false;
        }
        
        return false;
    }
}
