using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/MakeRainAction")]
public class MakeRainAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();
    
        if (stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {
            Debug.Log("It is ");
            player.RainParticleSystem.SetActive(true);
        }
        else
        {
            Debug.Log("It isnt ");
            player.RainParticleSystem.SetActive(false);
        }

    }

}
