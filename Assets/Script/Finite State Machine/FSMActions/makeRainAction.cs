using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/MakeRainAction")]
public class MakeRainAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();

        //if it isn't raining start rain
        if (stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {
            Debug.Log("It is now");

            player.RainParticleSystem.SetActive(true);
            player.StartCoroutine(player.ResetRain());
            //player.isRaining = true;
            Debug.Log("raining stopped");

        }
        else
        {


            Debug.Log("nahhhh");
        }
    }

}
