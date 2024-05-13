using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskLocateClosestGrass : BTNode
{
    NavMeshAgent thisAgent;
    Animal thisAnimal;

    public TaskLocateClosestGrass(Animal animal, NavMeshAgent agent)
    {
        thisAnimal = animal;
        thisAgent = agent;
    }


    protected override NodeState OnRun()
    {
        if(thisAnimal.grassSet.Items.Count <= 0)
        {
            state = NodeState.FAILURE;
        }
        else
        {
            float d = 1000;
            foreach(GameObject go in thisAnimal.grassSet.Items)
            {
                float distance = Mathf.Abs((thisAnimal.transform.position - go.transform.position).magnitude);
                if (distance < d)
                {
                    d = distance;
                    thisAnimal.SetClosestGrass(go);
                }

            }
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
