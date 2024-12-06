using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class plasticBagMonsterTree : BTree
{
    private PlasticBagMonster thisEnemy;

    protected override BTNode SetupTree()
    {
        thisEnemy = transform.GetComponent<PlasticBagMonster>();

        BTNode root = new Selector(new List<BTNode>
        {
            new Selector(new List<BTNode>
            {
                ////NOT STAGGERED SEQUENCE
                new Sequence(new List<BTNode>
                {
                    new Inverter(new CheckIfDying(thisEnemy)),
                    new Inverter(new CheckIfStaggered(thisEnemy)),
                    new Selector(new List<BTNode>()
                    {
                        //ATTACK PLANT SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckInSmotherRange(thisEnemy),
                            new Timer(2f, new TaskInitiateSmother(thisEnemy)),
                            new TaskSmotherPlant(thisEnemy),
                        }),

                        ////PATH TO PLANT SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfPlantSpotted(thisEnemy),
                            new TaskPathToPlant(thisEnemy),
                        }),

                        ////PATROL SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfDying(thisEnemy)),
                            new TaskPickRandomWaypoint(thisEnemy),
                            new TaskPathToWaypoint(thisEnemy),
                            new Timer(20f, new TaskFloat(thisEnemy))
                        })
                    })
                }),

                ////WAIT OUT STAGGER AND DEATH
                new Sequence(new List<BTNode>
                {
                    new TaskAwaitDeath(thisEnemy)
                })
            })

        });
        return root;

    }
}

