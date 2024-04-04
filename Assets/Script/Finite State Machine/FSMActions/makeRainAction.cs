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
            //Debug.Log("It is now raining ");

            player.RainParticleSystem.SetActive(true);
           // player.StartCoroutine(player.ResetRain());
            //player.isRaining = true;
  

        }
        else if (!stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {

            player.RainParticleSystem.SetActive(false);
            //Debug.Log("raining stopped");
        }
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
