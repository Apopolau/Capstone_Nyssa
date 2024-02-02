using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/TakeHitAction")]
public class TakeHitAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        CelestialPlayer player = stateMachine.GetComponent<CelestialPlayer>();

        int currHealthPoint = player.GetHealth();
        player.SetHealth(currHealthPoint -= 10);
    }
}
