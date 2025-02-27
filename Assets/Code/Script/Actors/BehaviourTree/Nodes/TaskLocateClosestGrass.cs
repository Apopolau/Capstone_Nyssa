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
        if (thisAnimal.GetIsKidnapped())
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (thisAnimal.GetGrassSet().Items.Count <= 0)
        {
            state = NodeState.FAILURE;
        }
        else
        {
            float d = 1000;
            foreach(GameObject go in thisAnimal.GetGrassSet().Items)
            {
                float distance = Mathf.Abs((thisAnimal.transform.position - go.transform.position).magnitude);
                if (distance < d)
                {
                    d = distance;
                    thisAnimal.SetClosestGrass(go);
                }

            }
            if(d < 1000)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
            }
        }
        return state;
    }


    protected override void OnReset()
    {

    }
}
