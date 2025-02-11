using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class invadingOilSpillTree : BTree
{
    private OilMonster thisEnemy;

    protected override BTNode SetupTree()
    {
        thisEnemy = transform.GetComponent<OilMonster>();

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
                        ////ATTACK PLAYER SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfPlayerDead(thisEnemy)),
                            new CheckInAttackRange(thisEnemy),
                            new Inverter(new CheckIfPlayerDead(thisEnemy)),
                            new Timer(thisEnemy.GetEnemyAnimator().GetAnimationLength("attack"), new taskInitiateAttack(thisEnemy)),
                            new TaskEndAttack(thisEnemy)
                        }),

                        ////KIDNAP ANIMAL SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckInKidnapRange(thisEnemy),
                            new Inverter(new CheckIfStuck(thisEnemy)),
                            new TaskKidnapAnimal(thisEnemy),
                            new TaskFindEscapeRoute(thisEnemy),
                            new TaskHeadOut(thisEnemy)
                        }),

                        ////PATH TO ANIMAL SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnimalSpotted(thisEnemy),
                            new Inverter(new CheckIfStuck(thisEnemy)),
                            new TaskPathToAnimal(thisEnemy)
                        }),

                        ////CHASE PLAYER SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfPlayerDead(thisEnemy)),
                            new CheckPlayerInRange(thisEnemy),
                            new Inverter(new CheckIfPlayerDead(thisEnemy)),
                            new taskChase(thisEnemy)
                        }),
         
                        ////PATROL SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new TaskInvadePatrol(thisEnemy),
                        })
                    })
                }),

                ////WAIT OUT STAGGER OR DEATH ANIMATION
                new Sequence(new List<BTNode>
                {
                    new TaskAwaitDeath(thisEnemy)
                })
            })
        });
        return root;
    }
}
