using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskRunAwayFromTarget : BTNode
{
    private GameObjectRuntimeSet thingsToRunFrom;
    Actor thisActor;
    //NavMeshAgent thisAgent;
    //OurAnimator thisAnimator;
    float runRange;

    public TaskRunAwayFromTarget(Actor actor, GameObjectRuntimeSet runtimeSet, float range)
    {
        thisActor = actor;
        //thisAgent = navAgent;
        thingsToRunFrom = runtimeSet;
        //thisAnimator = animator;
        runRange = range;
    }

    protected override NodeState OnRun()
    {
        List<GameObject> objectsInRange = new List<GameObject>();
        foreach(GameObject go in thingsToRunFrom.Items)
        {
            bool inRange = Mathf.Abs((thisActor.transform.position - go.transform.position).magnitude) <= runRange;
            if (inRange)
            {
                objectsInRange.Add(go);
            }
        }

        if(objectsInRange.Count > 0)
        {
            //Vector3 fleeVector = location - this.transform.position;
            List<Vector3> directionVectors = new List<Vector3>();
            Vector3 finalVector = Vector3.zero;
            foreach(GameObject go in objectsInRange)
            {
                directionVectors.Add(thisActor.transform.position - go.transform.position);
                finalVector += directionVectors[directionVectors.Count - 1];
            }
            finalVector.Normalize();

            //if(thisActor.GetComponent<NavMeshAgent>().T)
            thisActor.SetAgentPath(finalVector);
            state = NodeState.RUNNING;
        }
        else
        {
            thisActor.ResetAgentPath();
            state = NodeState.SUCCESS;
        }

        return state;
    }


    protected override void OnReset()
    {

    }
}
