using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class DuckTree : BTree
{
    Duck thisDuck;
    NavMeshAgent agent;
    GameObject managerObject;
    WeatherState weatherState;
    AnimalAnimator duckAnimator;

    protected override BTNode SetupTree()
    {
        thisDuck = GetComponent<Duck>();
        agent = GetComponent<NavMeshAgent>();
        weatherState = thisDuck.weatherState;
        duckAnimator = thisDuck.animalAnimator;

        List<int> nonEatingAnimations;
        nonEatingAnimations = new List<int>();
        nonEatingAnimations.Add(duckAnimator.IfPanickingHash);
        nonEatingAnimations.Add(duckAnimator.IfWalkingHash);
        nonEatingAnimations.Add(duckAnimator.IfSwimmingHash);

        List<int> nonPanickingAnimations;
        nonPanickingAnimations = new List<int>();
        nonPanickingAnimations.Add(duckAnimator.IfEatingHash);
        nonPanickingAnimations.Add(duckAnimator.IfWalkingHash);
        nonPanickingAnimations.Add(duckAnimator.IfSwimmingHash);

        List<int> nonWalkingAnimations;
        nonWalkingAnimations = new List<int>();
        nonWalkingAnimations.Add(duckAnimator.IfEatingHash);
        nonWalkingAnimations.Add(duckAnimator.IfPanickingHash);
        nonWalkingAnimations.Add(duckAnimator.IfSwimmingHash);

        List<int> nonSwimmingAnimations;
        nonSwimmingAnimations = new List<int>();
        nonSwimmingAnimations.Add(duckAnimator.IfEatingHash);
        nonSwimmingAnimations.Add(duckAnimator.IfPanickingHash);
        nonSwimmingAnimations.Add(duckAnimator.IfWalkingHash);

        BTNode root = new Selector(new List<BTNode>
        {
            //Pick our appropriate tree based on time of day
            new Selector(new List<BTNode>
            {
                //Duck near an enemy drops everything to run or hide from them, regardless of time of day
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    //Run to nearest shelter
                    new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.enemySet, 20),
                    new Selector(new List<BTNode>
                    {
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisDuck),
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
                    new CheckIfNight(weatherState),
                    //Pick between hiding in the shelter and hiding in tall grass
                    new Selector(new List<BTNode>
                    {
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisDuck),
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
                    new Inverter(new CheckIfNight(weatherState)),
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
                            new CheckIfAnyFood(thisDuck),
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.buildSet, 5),
                            //Figure out which food is closest
                            //Path to that food
                            new Timer(2f, new TaskInitiateAnimation(duckAnimator.animator, duckAnimator.IfEatingHash, nonEatingAnimations)),
                            new TaskRestoreStat(thisDuck.hunger)
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisDuck),
                            new CheckIfAnyWater(thisDuck),
                            new taskInitiatePathTo(agent, thisDuck.waterWaypoint.transform, duckAnimator),
                            new Timer(2f, new TaskInitiateAnimation(duckAnimator.animator, duckAnimator.IfEatingHash, nonEatingAnimations)),
                            new TaskRestoreStat(thisDuck.thirst)
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisDuck),
                            //Find some nice behaviours for a bored duck
                            new TaskRestoreStat(thisDuck.entertained)
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
