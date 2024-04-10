using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/MakeRainAction")]
public class MakeRainAction : FSMAction
{
    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }


    public override void Execute(BaseStateMachine stateMachine)
    {

        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {

            player.RainParticleSystem.SetActive(true);


        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {

            player.RainParticleSystem.SetActive(false);
        }
        stateMachine.GetComponent<CelestialPlayer>().buttonRain = false;


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }
}