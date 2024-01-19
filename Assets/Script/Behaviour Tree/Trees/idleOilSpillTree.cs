using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class idleOilSpillTree : BTree
{
    private CelestialPlayer player;
    private Enemy enemy;
    private NavMeshAgent enemyMeshAgent;

    //Enemy Movements
    public Transform[] waypoints;
    public static float speed = 2f;

    //Enemy Health
    [SerializeField] private float startingHealth=10;
    [SerializeField] private float currHealth;
    [SerializeField] private Transform playerTransform;
    
    //collider attackCollider or attack range
    //collider chase collider or chase range

    protected override BTNode SetupTree()
    {
       enemyMeshAgent = transform.GetComponent<NavMeshAgent>();
       player = transform.GetComponent<Enemy>().playerObj.GetComponent<CelestialPlayer>();
       enemy = transform.GetComponent<Enemy>();
        //Remove this when you actually implement it
        //throw new System.NotImplementedException();


        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BTNode>
        {
           new Sequence(new List<BTNode>
            {   new CheckInAttackRange(playerTransform,enemyMeshAgent,player),
                new TaskAttack(playerTransform,enemyMeshAgent,player),


            }),



          new Sequence(new List<BTNode>
            {
                 //check if anything is in the range of the enemy
            // new CheckIfAnyInRange(enemyMeshAgent),
            
                 //check if player is in the enemy range
                // new CheckIfPlayerIsVisible(enemyMeshAgent),
                 new CheckInRange(enemyMeshAgent),
                 //new TaskAttackPlayer(enemyMeshAgent,player)
                  //CHASE THE PLAYER
                  new taskChase(playerTransform,enemyMeshAgent,player),


             }),
           

          /*MAKE ANOTHER ONE TO RESPOND TO ATTACKS*/

            /*
        if they can see the player (collider)
        if close enough to player
        Attack player
        if they can see the player/not close enough
        Run at player

        if nothing else
        switch to wander behaviour
            */
            ////PATROL SEQUENCE
            new CheckInRange(enemyMeshAgent),
            new TaskPatrol( enemy,enemyMeshAgent, transform, waypoints),


        });
        return root;
        
    }
}
