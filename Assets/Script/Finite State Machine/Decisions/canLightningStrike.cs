using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/CanLightningStrike")]
public class CanLightningStrike: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); 
        if (stateMachine.GetComponent<CelestialPlayer>().isAttacking && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike && stateMachine.GetComponent<PowerBehaviour>().LightningStats.isEnabled)
        {
            //Debug.Log("currently doing lightning attack");

            return true;
        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking)
        {
       
            return false;
        }
        return false;
    }
}