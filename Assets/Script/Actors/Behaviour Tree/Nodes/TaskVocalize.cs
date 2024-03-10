using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskVocalize : BTNode
{
    GameObject player;
    NavMeshAgent thisAgent;
    float playerRange;

    public TaskVocalize(NavMeshAgent thisAgent, GameObject player, float range)
    {
        this.thisAgent = thisAgent;
        this.player = player;
        playerRange = range;
    }


    protected override NodeState OnRun()
    {
        float distance = (thisAgent.transform.position - player.transform.position).magnitude;
        if(distance <= playerRange)
        {
            thisAgent.GetComponent<Transform>().LookAt(player.transform);
            //Do a popup here
            //Also do a sound, like a quack
            state = NodeState.RUNNING;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        
        return state;
    }


    protected override void OnReset()
    {

    }
}
