using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class CheckIfPlayerDead : BTCondition
{
    CelestialPlayer thisPlayer;

    public CheckIfPlayerDead(CelestialPlayer player)
    {
        thisPlayer = player;
    }

    protected override NodeState OnRun()
    {
        if (thisPlayer.isDead)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected override void OnReset() { }
}
