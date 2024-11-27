using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class smogMonsterTreeInvader: BTree
{
    private CelestialPlayer player;
    private SmogMonster enemy;
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
        enemy = transform.GetComponent<SmogMonster>();
        rb = GetComponent<Rigidbody>();
        //waypointPath = GetComponent<EnemyInvadingPath>();

        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BTNode>
        {
            //ATTACK PLAYER SEQUENCE
            new Sequence(new List<BTNode>
            {
                new Inverter(new CheckIfDying(enemy)),
                new Inverter(new CheckIfStaggered(enemy)),
                new CheckInAttackRange(enemy),
                new Inverter(new CheckIfPlayerDead(enemy)),
                new Timer(enemy.GetEnemyAnimator().GetAnimationLength("attack"), new taskInitiateAttack(enemy)),
                new TaskEndAttack(enemy)
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
              new Inverter(new CheckIfDying(enemy)),
              new Inverter(new CheckIfStaggered(enemy)),
              new CheckInKidnapRange(enemy),
             // new Timer(2f, new TaskInitiateSmother(enemy)),
              new TaskKidnapAnimal(enemy),
             new TaskFindEscapeRoute(enemy),
              new TaskHeadOut(enemy,enemyMeshAgent,transform),
              //if animal is kidnapped 
              //get closests waypoint 
              //bring animal with you 





        }),
               //PATHTOAnimal
            new Sequence(new List<BTNode>
           {
                new Inverter(new CheckIfDying(enemy)),
              new Inverter(new CheckIfStaggered(enemy)),
              new CheckIfAnimalSpotted(enemy),
              new TaskPathToAnimal(enemy),





           }),


            new Sequence(new List<BTNode>
            {
                //check if anything is in the range of the enemy
                // new CheckIfAnyInRange(enemyMeshAgent),
            
                //check if player is in the enemy range
                // new CheckIfPlayerIsVisible(enemyMeshAgent),
                new Inverter(new CheckIfDying(enemy)),
                new Inverter(new CheckIfStaggered(enemy)),
                new CheckInRange(enemy),
                new Inverter(new CheckIfPlayerDead(enemy)),
                //new TaskAttackPlayer(enemyMeshAgent,player)
                //CHASE THE PLAYER
                new taskChase(enemy)
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
                //new Inverter(new CheckIfPathSelected(enemy)),
                //new TaskInvadeChoosePath(enemy),
                new TaskInvadePatrol(enemy,enemyMeshAgent,transform),
          
         

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
