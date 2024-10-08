using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class plasticBagMonsterTree : BTree
{
    private CelestialPlayer player;
    private PlasticBagMonster enemy;
    private NavMeshAgent enemyMeshAgent;
    Rigidbody rb;
    //Enemy Movements
    public static float speed = 2f;



    protected override BTNode SetupTree()
    {
        enemyMeshAgent = transform.GetComponent<NavMeshAgent>();
        //player = transform.GetComponent<Enemy>().celestialPlayer;
        enemy = transform.GetComponent<PlasticBagMonster>();
        rb = GetComponent<Rigidbody>();
        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BTNode>
        {
            //ATTACK PLANT SEQUENCE
            
        new Sequence(new List<BTNode>
        {
              new Inverter(new CheckIfDying(enemy)),
              new Inverter(new CheckIfStaggered(enemy)),
              new CheckInSmotherRange(enemy),
              new Timer(2f, new TaskInitiateSmother(enemy)),
              new TaskSmotherPlant(enemy),
        }),

           new Sequence(new List<BTNode>
           {
                new Inverter(new CheckIfDying(enemy)),
              new Inverter(new CheckIfStaggered(enemy)),
              new CheckIfPlantSpotted(enemy),
              new TaskPathToPlant(enemy),
           }),

        new Sequence(new List<BTNode>
        {
            ////PATROL SEQUENCE
            new Inverter(new CheckIfDying(enemy)),
            new TaskFloat( enemy,rb,enemyMeshAgent, transform),


        }),
          
               
            new Sequence(new List<BTNode>
            {
                new TaskAwaitDeath(enemyMeshAgent)
            })

        });
        return root;

    }
}

