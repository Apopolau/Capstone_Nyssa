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
    Rigidbody rb;
    //Enemy Movements
    public Transform[] waypoints;
    public static float speed = 2f;

    //Enemy Health
    [SerializeField] private float startingHealth = 10;
    [SerializeField] private float currHealth;
    [SerializeField] private Transform playerTransform;

    //collider attackCollider or attack range
    //collider chase collider or chase range

    protected override BTNode SetupTree()
    {
        enemyMeshAgent = transform.GetComponent<NavMeshAgent>();
        player = transform.GetComponent<Enemy>().playerObj.GetComponent<CelestialPlayer>();
        enemy = transform.GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();

        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new Inverter(new CheckIfDying(enemy)),
                new Inverter(new CheckIfStaggered(enemy)),
                new CheckInAttackRange(playerTransform,enemyMeshAgent,player),
                new Inverter(new CheckIfPlayerDead(player)),
                new Timer(2f, new taskInitiateAttack(playerTransform,enemyMeshAgent,player)),
                new TaskAttack(playerTransform,enemyMeshAgent,player,transform),
            }),

            new Sequence(new List<BTNode>
            {
                //check if anything is in the range of the enemy
                // new CheckIfAnyInRange(enemyMeshAgent),
            
                //check if player is in the enemy range
                // new CheckIfPlayerIsVisible(enemyMeshAgent),
                new Inverter(new CheckIfDying(enemy)),
                new Inverter(new CheckIfStaggered(enemy)),
                new CheckInRange(enemyMeshAgent),
                new Inverter(new CheckIfPlayerDead(player)),
                //new TaskAttackPlayer(enemyMeshAgent,player)
                //CHASE THE PLAYER
                new taskChase(playerTransform,enemyMeshAgent,player,transform),
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
         
            new Sequence(new List<BTNode>
            {
          
                ////PATROL SEQUENCE
                new Inverter(new CheckIfDying(enemy)),
                new TaskPatrol( enemy,rb,enemyMeshAgent, transform, waypoints),
            }),

            new Sequence(new List<BTNode>
            {
                new TaskAwaitDeath(enemyMeshAgent)
            })

        });
        return root;

    }
}
