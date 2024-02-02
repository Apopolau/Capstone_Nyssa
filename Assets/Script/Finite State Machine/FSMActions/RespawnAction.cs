using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/RespawnAction")]
public class RespawnAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();

        if (!stateMachine.GetComponent<CelestialPlayer>().isRespawning)
        {

            player.SetHealth((int)100);
            player.SetLocation(player.OrigPos);


        }


    }
}

