using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Architecture/FSM/Decisions/canBasicAttack")]
public class CanBasicAttack: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {

        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();


        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        //if (stateMachine.GetComponent<CelestialPlayer>().buttonBasicAttack && stateMachine.GetComponent<PowerBehaviour>().BasicAttackStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().canBasicAttack && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC)
        if (stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.BASIC)
        {
            //Debug.Log("Attempting to use Basic Attack");
            stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;
            return true;
        }
        stateMachine.GetComponent<CelestialPlayer>().buttonBasicAttack=false;
        return false;
    }
}