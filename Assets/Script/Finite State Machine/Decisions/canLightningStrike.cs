using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/canLightiningStrike")]
public class CanLightningStrike: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking)
        {
            Debug.Log("currently doing lightning attack");

            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking)
        {
       
            return false;
        }
        return false;
    }
}