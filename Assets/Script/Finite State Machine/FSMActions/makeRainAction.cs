using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Architecture/FSM/Actions/MakeRainAction")]
public class MakeRainAction : FSMAction
{
    CelestialPlayer player;
    HUDManager hudManager;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
        hudManager = player.GetHudManager();
    }


    public override void Execute(BaseStateMachine stateMachine)
    {

        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {

            player.RainParticleSystem.SetActive(true);
            player.StartCoroutine(player.DrainRainEnergy());


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