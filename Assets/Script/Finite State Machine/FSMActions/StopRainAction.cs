using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/StopRainAction")]
public class StopRainAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();

        //if it isn't raining start rain
        Debug.Log("******It is no longer raining");
        player.RainParticleSystem.SetActive(false);
        player.isRaining = false;
        


    }

}
