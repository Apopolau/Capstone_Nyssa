using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskFollowPlayer : BTNode
{
    GameObject player;
    Actor thisActor;
    Vector3 destination;
    //OurAnimator thisAnimator;

    public TaskFollowPlayer(Actor thisActor, GameObject player, Vector3 targetDestination)
    {
        this.thisActor = thisActor;
        this.player = player;
        destination = targetDestination;
        //thisAnimator = animator;
    }


    protected override NodeState OnRun()
    {
        float distanceToFinal = (thisActor.transform.position - destination).magnitude;
        if(distanceToFinal < 20)
        {
            //thisAnimator.ToggleSetWalk();
            thisActor.GetComponent<OurAnimator>().SetAnimationFlag("move", false);
            thisActor.GetComponent<Animal>().SetEscort(false);
            thisActor.ResetAgentPath();
            state = NodeState.SUCCESS;
            return state;
        }

        if (thisActor.GetComponent<Animal>())
        {
            if(!thisActor.GetComponent<Animal>().GetIsEscorted() || thisActor.GetComponent<Animal>().GetIsScared() || thisActor.GetComponent<Animal>().GetIsKidnapped())
            {
                thisActor.GetComponent<OurAnimator>().SetAnimationFlag("move", false);
                thisActor.ResetAgentPath();
                state = NodeState.FAILURE;
                return state;
            }
        }



        thisActor.GetComponent<NavMeshAgent>().destination = player.transform.position;
        //thisAnimator.ToggleSetWalk();
        thisActor.GetComponent<OurAnimator>().SetAnimationFlag("move", true);

        state = NodeState.RUNNING;

        if (thisActor.GetComponent<NavMeshAgent>().path.status == NavMeshPathStatus.PathInvalid)
        {
            //thisAnimator.ToggleSetWalk();
            thisActor.GetComponent<OurAnimator>().SetAnimationFlag("move", false);
            state = NodeState.FAILURE;
        }

        return state;
    }


    protected override void OnReset()
    {

    }
}
