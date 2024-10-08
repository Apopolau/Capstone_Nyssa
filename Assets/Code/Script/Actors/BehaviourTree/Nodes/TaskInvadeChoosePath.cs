using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskInvadeChoosePath : BTNode
{
    KidnappingEnemy thisEnemy;
    private List<Transform> waypoints;
    public List<List<Transform>> pathsList = new List<List<Transform>>();
 
    public TaskInvadeChoosePath(KidnappingEnemy enemy)
    {
        thisEnemy = enemy;
    }

    protected override NodeState OnRun()
    {
        if (thisEnemy.GetIsPathSelected())
        {
            //waypoints = thisEnemy.invaderEnemyRoutes.getRandomPath();
            state = NodeState.RUNNING;
        }
        else
        {
            thisEnemy.GetInvaderEnemyRoutes().LoadPathList();
            waypoints = thisEnemy.GetInvaderEnemyRoutes().GetComponent<EnemyInvadingPath>().getRandomPath();
            thisEnemy.GetInvaderEnemyRoutes().setEnemiesPath( thisEnemy, waypoints );
            thisEnemy.SetIsPathSelected(true);
            state = NodeState.SUCCESS;
        }
        return state;
    }
    protected override void OnReset()
    {
      
    }
}
