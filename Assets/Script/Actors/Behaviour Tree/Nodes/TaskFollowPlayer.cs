using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskFollowPlayer : BTNode
{
    GameObject player;
    NavMeshAgent thisAgent;
    Vector3 destination;
    OurAnimator thisAnimator;

    public TaskFollowPlayer(NavMeshAgent thisAgent, GameObject player, Vector3 targetDestination, OurAnimator animator)
    {
        this.thisAgent = thisAgent;
        this.player = player;
        destination = targetDestination;
        thisAnimator = animator;
    }


    protected override NodeState OnRun()
    {
        float distanceToFinal = (thisAgent.transform.position - destination).magnitude;
        if(distanceToFinal < 20)
        {
            thisAnimator.ToggleSetWalk();
            thisAgent.GetComponent<Animal>().SetEscort(false);
            thisAgent.ResetPath();
            state = NodeState.SUCCESS;
            return state;
        }

        if (thisAgent.GetComponent<Animal>())
        {
            if(!thisAgent.GetComponent<Animal>().isEscorted || thisAgent.GetComponent<Animal>().isScared || thisAgent.GetComponent<Animal>().isKidnapped)
            {
                thisAnimator.ToggleSetWalk();
                thisAgent.ResetPath();
                state = NodeState.FAILURE;
                return state;
            }
        }



        thisAgent.destination = player.transform.position;
        thisAnimator.ToggleSetWalk();

        state = NodeState.RUNNING;

        if (thisAgent.path.status == NavMeshPathStatus.PathInvalid)
        {
            thisAnimator.ToggleSetWalk();
            state = NodeState.FAILURE;
        }

        return state;
    }


    protected override void OnReset()
    {

    }
}
