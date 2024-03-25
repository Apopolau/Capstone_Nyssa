using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/StopRainAction")]
public class StopRainAction : FSMAction
{
    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {

        //if it isn't raining start rain
       // Debug.Log("******It is no longer raining");
        //player.RainParticleSystem.SetActive(false);
       // player.isRaining = false;
        
        //////////////////////////////////////////////

    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
