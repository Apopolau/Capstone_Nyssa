using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class DuckTree : BTree
{
    Duck thisDuck;
    NavMeshAgent agent;
    DayNightCycle dayNightCycle;
    AnimalAnimator duckAnimator;

    protected override BTNode SetupTree()
    {
        thisDuck = GetComponent<Duck>();
        agent = GetComponent<NavMeshAgent>();
        dayNightCycle = thisDuck.dayNightCycle;
        duckAnimator = thisDuck.animalAnimator;

        BTNode root = new Selector(new List<BTNode>
        {
            //Pick our appropriate tree based on time of day
            new Selector(new List<BTNode>
            {
                //If it's night time
                new Sequence(new List<BTNode>
                {
                    new CheckIfNight(dayNightCycle)
                }),

                //If it's not night time
                new Sequence(new List<BTNode>
                {
                    new Inverter(new CheckIfNight(dayNightCycle)),
                    new Selector(new List<BTNode>
                    {
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfHungry(thisDuck),
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.buildSet, 5)
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisDuck),
                            new taskInitiatePathTo(agent, thisDuck.waterWaypoint.transform)
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisDuck)

                        })
                    })
                })
            })

            //If Day time
        
        }) ;
        return root;
        
    }
}
