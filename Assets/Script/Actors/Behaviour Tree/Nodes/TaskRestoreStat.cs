using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskRestoreStat : BTNode
{
    Stat stat;

    public TaskRestoreStat(Stat statToRestore)
    {
        stat = statToRestore;
    }

    //Restores a stat like hunger or life to full
    protected override NodeState OnRun()
    {
        stat.current = stat.max;
        stat.low = false;
        state = NodeState.SUCCESS;
        return state;
    }



    protected override void OnReset()
    {
    }
}
