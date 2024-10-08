using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class smogMonsterTreeInvader: BTree
{
    private CelestialPlayer player;
    private SmogMonster smogMonster;
    private NavMeshAgent enemyMeshAgent;
    Rigidbody rb;
    public static float speed = 2f;
    //public EnemyInvadingPath waypointPath;
    //Enemy Health
    //[SerializeField] private float startingHealth = 10;
    [SerializeField] private float currHealth;
    [SerializeField] private Transform playerTransform;
    //[SerializeField] public List<Transform> waypointPath;
    //collider attackCollider or attack range
    //collider chase collider or chase range

    protected override BTNode SetupTree()
    {
        enemyMeshAgent = transform.GetComponent<NavMeshAgent>();
        //player = transform.GetComponent<SmogMonster>().celestialPlayer;
        smogMonster = transform.GetComponent<SmogMonster>();
        rb = GetComponent<Rigidbody>();
        //waypointPath = GetComponent<EnemyInvadingPath>();

        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BTNode>
        {
            //ATTACK PLAYER SEQUENCE
            new Sequence(new List<BTNode>
            {
                new Inverter(new CheckIfDying(smogMonster)),
                new Inverter(new CheckIfStaggered(smogMonster)),
                new CheckInAttackRange(smogMonster),
                new Inverter(new CheckIfPlayerDead(smogMonster)),
                new Timer(2f, new taskInitiateAttack(smogMonster)),
                new TaskAttack(smogMonster)
            }),

              //ESCAPE SEQUENCE
         /*       new Sequence(new List<BTNode>
        {
              new Inverter(new CheckIfAnimalKidnapped(enemy)),
              new Inverter(new CheckIfDying(enemy)),
              new Inverter(new CheckIfStaggered(enemy)),
          
              new TaskFindEscapeRoute(enemy),
              new TaskHeadOut(enemy)
             *



        }),*/




            //KIDNAP ANIMAL SEQUENCE
                new Sequence(new List<BTNode>
        {
              new Inverter(new CheckIfDying(smogMonster)),
              new Inverter(new CheckIfStaggered(smogMonster)),
              new CheckInKidnapRange(smogMonster),
             // new Timer(2f, new TaskInitiateSmother(enemy)),
              new TaskKidnapAnimal(smogMonster),
             new TaskFindEscapeRoute(smogMonster),
              new TaskHeadOut(smogMonster,enemyMeshAgent,transform),
              //if animal is kidnapped 
              //get closests waypoint 
              //bring animal with you 





        }),
               //PATHTOAnimal
            new Sequence(new List<BTNode>
           {
                new Inverter(new CheckIfDying(smogMonster)),
              new Inverter(new CheckIfStaggered(smogMonster)),
              new CheckIfAnimalSpotted(smogMonster),
              new TaskPathToAnimal(smogMonster),





           }),


            new Sequence(new List<BTNode>
            {
                //check if anything is in the range of the enemy
                // new CheckIfAnyInRange(enemyMeshAgent),
            
                //check if player is in the enemy range
                // new CheckIfPlayerIsVisible(enemyMeshAgent),
                new Inverter(new CheckIfDying(smogMonster)),
                new Inverter(new CheckIfStaggered(smogMonster)),
                new CheckInRange(smogMonster),
                new Inverter(new CheckIfPlayerDead(smogMonster)),
                //new TaskAttackPlayer(enemyMeshAgent,player)
                //CHASE THE PLAYER
                new taskChase(smogMonster)
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
                new Inverter(new CheckIfDying(smogMonster)),
                new Inverter(new CheckIfPathSelected(smogMonster)),
                new TaskInvadeChoosePath(smogMonster),
                new TaskInvadePatrol(smogMonster,enemyMeshAgent,transform),
          
         

                //take in a series or route
                // chooses a random route
                //follows the random route
                //if player spotter break and follow
                // if animal spotted break adn atack or break and kidnap
                // if route complete, hover around for a bit the follow a new route that branches and checks the other branches
               // new TaskInvasionPatrol( enemy,rb,enemyMeshAgent, transform, waypoints),
                //take waypoints that are located with the scene and simply go through those
                //if they are close to an animal kidnapp
                // if they are close to a person attack
            }),

            new Sequence(new List<BTNode>
            {
                new TaskAwaitDeath(enemyMeshAgent)
            })

        }) ;
        return root;

    }
}
