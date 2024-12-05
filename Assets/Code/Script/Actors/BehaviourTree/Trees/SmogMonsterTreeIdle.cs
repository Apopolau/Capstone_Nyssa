using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class smogMonsterTree : BTree
{
    private SmogMonster thisEnemy;
    public Transform[] waypoints;

    protected override BTNode SetupTree()
    {
        thisEnemy = transform.GetComponent<SmogMonster>();

        // Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
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
                            new Timer(thisEnemy.GetEnemyAnimator().GetAnimationLength("attack"), new taskInitiateAttack(thisEnemy)),
                            new TaskEndAttack(thisEnemy)
                        }),
                        ////CHASE PLAYER SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfPlayerDead(thisEnemy)),
                            new CheckPlayerInRange(thisEnemy),
                            new taskChase(thisEnemy)
                        }),
                        ////PATROL SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfDying(thisEnemy)),
                            new Inverter(new CheckIfStaggered(thisEnemy)),
                            new TaskPatrol( thisEnemy, waypoints),
                        })
                    })
                }),

                ////STAGGER AND DEATH SEQUENCE
                new Sequence(new List<BTNode>
                {
                    new TaskAwaitDeath(thisEnemy)
                })
            })
        });
        return root;
    }
}
