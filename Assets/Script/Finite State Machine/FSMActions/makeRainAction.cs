using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/MakeRain")]
public class MakeRainAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        playerMovement player = stateMachine.GetComponent<playerMovement>();
    
        

    }

}
