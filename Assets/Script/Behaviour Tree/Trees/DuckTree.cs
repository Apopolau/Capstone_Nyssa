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
                //Duck near an enemy drops everything to run or hide from them
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    //Run to nearest shelter
                    new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.enemySet, 20),
                    new Selector(new List<BTNode>
                    {
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.shelterWaypoint, 20),
                            new taskInitiatePathTo(agent, thisDuck.shelterWaypoint.transform, duckAnimator)
                            //Hide in shelter
                            //Turn on hidden status
                        }),
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.buildSet, 20),
                            new taskInitiatePathTo(agent, thisDuck.shelterWaypoint.transform, duckAnimator)
                            //If not, find the nearest tall grass
                            //path to that grass
                            //Hide in it
                            //Turn on hidden status
                        }),
                    })
                }),
                

                //Duck should follow earth player if escort ability is used

                //If it's night time
                new Sequence(new List<BTNode>
                {
                    new CheckIfNight(dayNightCycle),
                    //Pick between hiding in the shelter and hiding in tall grass
                    new Selector(new List<BTNode>
                    {
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.shelterWaypoint, 20),
                            new taskInitiatePathTo(agent, thisDuck.shelterWaypoint.transform, duckAnimator)
                            //Hide in shelter
                            //Turn on hidden status
                        }),
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.buildSet, 20),
                            new taskInitiatePathTo(agent, thisDuck.shelterWaypoint.transform, duckAnimator)
                            //If not, find the nearest tall grass
                            //path to that grass
                            //Hide in it
                            //Turn on hidden status
                        }),
                    })
                }),

                //If it's not night time
                new Sequence(new List<BTNode>
                {
                    new Inverter(new CheckIfNight(dayNightCycle)),
                    new Selector(new List<BTNode>
                    {
                        new Sequence(new List<BTNode>
                        {
                            //Add a check for if the player is nearby
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.playerSet, 15),
                            //Check that they're not in the middle of an animation
                            //Have them look at the player, maybe add some kind of sound effect or visual acknowledgement
                        }),
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfHungry(thisDuck),
                            //Check if there is any food
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.buildSet, 5)
                            //Figure out which food is closest
                            //Path to that food
                            //Do the drinking/eating animation
                            //Reset duck's hunger value
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisDuck),
                            //Check if the water is clean
                            new taskInitiatePathTo(agent, thisDuck.waterWaypoint.transform, duckAnimator)
                            //Initiate a drinking animation when we get here
                            //Reset duck's thirst value
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisDuck)
                            //Find some nice behaviours for a bored duck
                            //Reset their boredom value once finished
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
