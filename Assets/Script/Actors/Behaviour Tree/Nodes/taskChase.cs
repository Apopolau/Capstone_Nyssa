using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class taskChase : BTNode
{
    //private Transform thisTarget;
    NavMeshAgent thisAgent;
    //CelestialPlayer thisPlayer;
    //Rigidbody rb;
    //private Transform transformPos;
    private Enemy thisEnemy;
    public taskChase(Enemy enemy)
    {
        thisEnemy = enemy;
        thisAgent = thisEnemy.GetComponent<NavMeshAgent>();
        //thisTarget = target;
        //thisPlayer = player;
        //transformPos = transform;
        
    }
    

    protected override NodeState OnRun()
    {


        float distance = Vector3.Distance(thisEnemy.GetClosestPlayer().transform.position, thisAgent.transform.position);

        if(thisEnemy.isStaggered || thisEnemy.isDying)
        {
            thisAgent.ResetPath();
            state = NodeState.FAILURE;
            return state;
        }

        if (!thisAgent.GetComponent<Enemy>().inAttackRange && thisAgent.GetComponent<Enemy>().seesPlayer)
       
        {


          thisAgent.SetDestination(thisEnemy.GetClosestPlayer().transform.position);
            thisEnemy.transform.LookAt(thisEnemy.GetClosestPlayer().transform.position);


            if (thisEnemy.inAttackRange)
            {
                state = NodeState.FAILURE;
            }
            state = NodeState.RUNNING;
        }
      
        else if (thisAgent.GetComponent<Enemy>().inAttackRange || !thisAgent.GetComponent<Enemy>().seesPlayer)
        {
            state = NodeState.FAILURE;
        }

            return state;


    }


    protected override void OnReset()
    {

    }
}
