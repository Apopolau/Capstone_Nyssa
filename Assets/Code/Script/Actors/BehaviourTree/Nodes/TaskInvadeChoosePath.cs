using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskInvadeChoosePath : BTNode
{
    KidnappingEnemy thisEnemy;
    //private List<Transform> waypoints;
    //public List<List<Transform>> pathsList = new List<List<Transform>>();

    public TaskInvadeChoosePath(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        
            //thisEnemy.GetInvaderEnemyRoutes().LoadPathList();
            //waypoints = thisEnemy.GetInvaderEnemyRoutes().getRandomPath();
            //thisEnemy.SetPath();
            //thisEnemy.SetIsPathSelected(true);
            state = NodeState.SUCCESS;
        

        return state;
    }
    protected override void OnReset()
    {

    }
}
