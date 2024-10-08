using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/FSM/Actions/TakeHitAction")]
public class TakeHitAction : FSMAction
{

    CelestialPlayer player;

    public override void EnterState(BaseStateMachine stateMachine)
    {
        player = stateMachine.GetComponent<CelestialPlayer>();
    }

    public override void Execute(BaseStateMachine stateMachine)
    {
        int currHealthPoint = player.GetHealth();
        player.SetHealth(currHealthPoint -= 10);
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }
}
