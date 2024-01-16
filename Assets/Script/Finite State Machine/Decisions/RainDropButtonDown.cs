using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "FSM/Decisions/RainDropButtonDown")]
public class RainDropButtonDown : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
       // CelestialPlayer celestialPlayer = stateMachine.GetComponent<CelestialPlayer>(); ;
     if(Input.GetKeyDown(KeyCode.Z) || stateMachine.GetComponent<CelestialPlayer>().isRaining)
        {
            Debug.Log("It's raining");
            //if (stateMachine.GetComponent<CelestialPlayer>().isRaining)

                //celestialPlayer.isRaining = true;
                return true;
        }
        // celestialPlayer.isRaining = false;
        Debug.Log("It's not raining");
        return false;
    }
}
