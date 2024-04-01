using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/CanLightningStrike")]
public class CanLightningStrike: Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        Debug.Log("Can LS Falseeeeeeee");
        PowerBehaviour attack;
        attack = stateMachine.GetComponent<PowerBehaviour>();
 
        if (stateMachine.GetComponent<CelestialPlayer>().buttonLightningStrike && stateMachine.GetComponent<CelestialPlayer>().canLightningStrike && stateMachine.GetComponent<PowerBehaviour>().LightningStats.isEnabled && stateMachine.GetComponent<CelestialPlayer>().powerInUse == CelestialPlayer.Power.LIGHTNINGSTRIKE)
        {
            stateMachine.GetComponent<CelestialPlayer>().isAttacking = true;

            Debug.Log("Can LS True");
            return true;
        }
       /* else if (!stateMachine.GetComponent<CelestialPlayer>().isAttacking || !attack.LightningStats.isEnabled || !stateMachine.GetComponent<CelestialPlayer>().canLightningStrike)
        { 
            Debug.Log("Can LS False");
     

      
            return false;
        }*/
        return false;
    }
}