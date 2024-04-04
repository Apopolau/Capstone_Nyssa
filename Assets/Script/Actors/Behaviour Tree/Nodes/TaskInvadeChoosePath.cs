using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskInvadeChoosePath : BTNode
{
    Enemy thisEnemy;
    private List<Transform> waypoints;
    public List<List<Transform>> pathsList = new List<List<Transform>>();
 
    public TaskInvadeChoosePath(Enemy enemy)
    {
        thisEnemy = enemy;


    }

    protected override NodeState OnRun()
    {
        if (thisEnemy.isPathSelected)
        {
            //waypoints = thisEnemy.invaderEnemyRoutes.getRandomPath();
            state = NodeState.RUNNING;
        }
        else
        {
            thisEnemy.invaderEnemyRoutes.LoadPathList();
            waypoints = thisEnemy.invaderEnemyRoutes.GetComponent<EnemyInvadingPath>().getRandomPath();
            thisEnemy.invaderEnemyRoutes.setEnemiesPath( thisEnemy, waypoints );
            thisEnemy.isPathSelected = true;
            state = NodeState.SUCCESS;
        }
        return state;
    }
    protected override void OnReset()
    {
      
    }
}
