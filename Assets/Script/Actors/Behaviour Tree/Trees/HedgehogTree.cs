using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class HedgehogTree : BTree
{
    Hedgehog thisHog;
    NavMeshAgent hogAgent;
    GameObject managerObject;
    WeatherState weatherState;
    AnimalAnimator hogAnimator;
    EarthPlayer earthPlayer;
    CelestialPlayer celestialPlayer;

    protected override BTNode SetupTree()
    {
        thisHog = GetComponent<Hedgehog>();
        hogAgent = GetComponent<NavMeshAgent>();
        earthPlayer = thisHog.GetEarthPlayer();
        celestialPlayer = thisHog.GetCelestialPlayer();
        weatherState = thisHog.weatherState;
        hogAnimator = thisHog.GetAnimator();

        List<int> nonEatingAnimations;
        nonEatingAnimations = new List<int>();
        nonEatingAnimations.Add(hogAnimator.IfPanickingHash);
        nonEatingAnimations.Add(hogAnimator.IfWalkingHash);
        nonEatingAnimations.Add(hogAnimator.IfSwimmingHash);

        List<int> nonPanickingAnimations;
        nonPanickingAnimations = new List<int>();
        nonPanickingAnimations.Add(hogAnimator.IfEatingHash);
        nonPanickingAnimations.Add(hogAnimator.IfWalkingHash);
        nonPanickingAnimations.Add(hogAnimator.IfSwimmingHash);

        List<int> nonWalkingAnimations;
        nonWalkingAnimations = new List<int>();
        nonWalkingAnimations.Add(hogAnimator.IfEatingHash);
        nonWalkingAnimations.Add(hogAnimator.IfPanickingHash);
        nonWalkingAnimations.Add(hogAnimator.IfSwimmingHash);

        List<int> nonSwimmingAnimations;
        nonSwimmingAnimations = new List<int>();
        nonSwimmingAnimations.Add(hogAnimator.IfEatingHash);
        nonSwimmingAnimations.Add(hogAnimator.IfPanickingHash);
        nonSwimmingAnimations.Add(hogAnimator.IfWalkingHash);

        BTNode root = new Selector(new List<BTNode>
        {
            //Pick our appropriate tree based on time of day
            new Selector(new List<BTNode>
            {
                ///
                ///STAY IN PLACE IF STUCK
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfStuck(thisHog),
                    new TaskAwaitDeath(hogAgent)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisHog.gameObject, thisHog.enemySet, 20),
                    //If there is an enemy, run or hide
                    new Selector(new List<BTNode>
                    {
                        //RUN!
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.enemySet, 10),
                            new TaskRunAwayFromTarget(hogAgent, thisHog.enemySet, hogAnimator, 10)
                        }),
                        //If not in immediate proximity of a monster, prioritize shelter first
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisHog),
                            new CheckIfInRangeOne(thisHog.gameObject, thisHog.shelterWaypoint, 20),
                            new taskInitiatePathTo(hogAgent, thisHog.shelterWaypoint.transform, hogAnimator),
                            new TaskHide(thisHog)
                        }),
                        //If no shelter, find grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.grassSet, 20),
                            new CheckForClosestGrass(thisHog, thisHog.grassSet, 20),
                            new taskInitiatePathToGrass(hogAgent, hogAnimator),
                            new TaskHide(thisHog)
                        }),

                    })
                }),
                
                ///
                ///FOLLOW EARTH PLAYER WHEN ESCORTED
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfEscorted(thisHog),
                    new TaskFollowPlayer(hogAgent, earthPlayer.gameObject, thisHog.shelterWaypoint.transform.position, hogAnimator)
                }),

                ///
                /// IF NONE OF THOSE THINGS, FOLLOW THESE BEHAVIOURS BY DEFAULT
                /// NIGHT TIME
                /// 
                new Sequence(new List<BTNode>
                {
                    new CheckIfNight(weatherState),
                    //Pick between hiding in the shelter and hiding in tall grass
                    new Selector(new List<BTNode>
                    {
                        //Shelter
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisHog),
                            new CheckIfInRangeOne(thisHog.gameObject, thisHog.shelterWaypoint, 20),
                            new taskInitiatePathTo(hogAgent, thisHog.shelterWaypoint.transform, hogAnimator),
                            new TaskHide(thisHog)
                        }),
                        //Tall grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.grassSet, 20),
                            new CheckForClosestGrass(thisHog, thisHog.grassSet, 20),
                            new taskInitiatePathToGrass(hogAgent, hogAnimator),
                            new TaskHide(thisHog)
                        }),
                    })
                }),


                ///
                /// IF NONE OF THOSE THINGS, FOLLOW THESE BEHAVIOURS BY DEFAULT
                /// DAY TIME
                /// 
                new Sequence(new List<BTNode>
                {
                    //Only run if not night
                    new Inverter(new CheckIfNight(weatherState)),
                    new Selector(new List<BTNode>
                    {
                        //Highest priority is to stop what you're doing and look at the player if they're close
                        new Sequence(new List<BTNode>
                        {
                            //Add a check for if the player is nearby
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.playerSet, 15),
                            new CheckIfAnimating(hogAnimator),
                            new CheckForClosestPlayer(thisHog, 15),
                            new Timer(3f, new TaskVocalize(hogAgent, thisHog, 15))
                        }),
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfHungry(thisHog),
                            //If hungry, is there anything to eat?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyFood(thisHog),
                                    new CheckIfInRangeAll(thisHog.gameObject, thisHog.buildSet, 5),
                                    new CheckForClosestFood(thisHog, 50),
                                    new taskInitiatePathToFood(hogAgent, hogAnimator),
                                    new Timer(2f, new TaskInitiateAnimation(hogAnimator.animator, hogAnimator.IfEatingHash, nonEatingAnimations)),
                                    new TaskRestoreStat(thisHog.hunger)
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyFood(thisHog)),
                                    new CheckForClosestPlayer(thisHog, 15),
                                    new Timer(3f, new TaskVocalize(hogAgent, thisHog, 15))
                                })
                            })
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisHog),
                            //If thirsty, is there anything to drink?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyWater(thisHog),
                                    new taskInitiatePathTo(hogAgent, thisHog.waterWaypoint.transform, hogAnimator),
                                    new Timer(2f, new TaskInitiateAnimation(hogAnimator.animator, hogAnimator.IfEatingHash, nonEatingAnimations)),
                                    new TaskRestoreStat(thisHog.thirst)
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisHog)),
                                    new CheckForClosestPlayer(thisHog, 15),
                                    new Timer(3f, new TaskVocalize(hogAgent, thisHog, 15))
                                })
                                
                            })
                            
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisHog),
                            //Find some nice behaviours for a bored duck
                            new TaskRestoreStat(thisHog.entertained)
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
