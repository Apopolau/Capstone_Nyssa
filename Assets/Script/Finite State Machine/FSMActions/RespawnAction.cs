using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/RespawnAction")]
public class RespawnAction : FSMAction
{

    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }


    public override void Execute(BaseStateMachine stateMachine)
    {
        if (!stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {

            player.SetHealth((int)100);
            player.SetLocation(player.OrigPos);


        }


    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}

