using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "FSM/Decisions/canBasicAttack")]
public class CanBasicAttack: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {

        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();


        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<PowerBehaviour>().BasicAttackStats.isEnabled)
        {
            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<PowerBehaviour>().BasicAttackStats.isEnabled)
        {
            return false;
        }
        return false;
    }
}