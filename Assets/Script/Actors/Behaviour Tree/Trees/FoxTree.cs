using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class FoxTree : BTree
{
    Fox thisFox;
    NavMeshAgent foxAgent;
    GameObject managerObject;
    WeatherState weatherState;
    AnimalAnimator foxAnimator;
    EarthPlayer earthPlayer;
    CelestialPlayer celestialPlayer;

    protected override BTNode SetupTree()
    {
        thisFox = GetComponent<Fox>();
        foxAgent = GetComponent<NavMeshAgent>();
        earthPlayer = thisFox.GetEarthPlayer();
        celestialPlayer = thisFox.GetCelestialPlayer();
        weatherState = thisFox.weatherState;
        foxAnimator = thisFox.GetAnimator();

        List<int> nonEatingAnimations;
        nonEatingAnimations = new List<int>();
        nonEatingAnimations.Add(foxAnimator.IfPanickingHash);
        nonEatingAnimations.Add(foxAnimator.IfWalkingHash);
        nonEatingAnimations.Add(foxAnimator.IfSwimmingHash);

        List<int> nonPanickingAnimations;
        nonPanickingAnimations = new List<int>();
        nonPanickingAnimations.Add(foxAnimator.IfEatingHash);
        nonPanickingAnimations.Add(foxAnimator.IfWalkingHash);
        nonPanickingAnimations.Add(foxAnimator.IfSwimmingHash);

        List<int> nonWalkingAnimations;
        nonWalkingAnimations = new List<int>();
        nonWalkingAnimations.Add(foxAnimator.IfEatingHash);
        nonWalkingAnimations.Add(foxAnimator.IfPanickingHash);
        nonWalkingAnimations.Add(foxAnimator.IfSwimmingHash);

        List<int> nonSwimmingAnimations;
        nonSwimmingAnimations = new List<int>();
        nonSwimmingAnimations.Add(foxAnimator.IfEatingHash);
        nonSwimmingAnimations.Add(foxAnimator.IfPanickingHash);
        nonSwimmingAnimations.Add(foxAnimator.IfWalkingHash);

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
                    new CheckIfStuck(thisFox),
                    new TaskAwaitDeath(foxAgent)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisFox.gameObject, thisFox.enemySet, 20),
                    //If there is an enemy, run or hide
                    new Selector(new List<BTNode>
                    {
                        //RUN!
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.enemySet, 10),
                            new TaskRunAwayFromTarget(foxAgent, thisFox.enemySet, foxAnimator, 10)
                        }),
                        //If not in immediate proximity of a monster, prioritize shelter first
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisFox),
                            new CheckIfInRangeOne(thisFox.gameObject, thisFox.shelterWaypoint, 20),
                            new taskInitiatePathTo(foxAgent, thisFox.shelterWaypoint.transform, foxAnimator),
                            new TaskHide(thisFox)
                        }),
                        //If no shelter, find grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.grassSet, 20),
                            new CheckForClosestGrass(thisFox, thisFox.grassSet, 20),
                            new TaskLocateClosestGrass(thisFox, foxAgent),
                            new taskInitiatePathToGrass(foxAgent, foxAnimator),
                            new TaskHide(thisFox)
                        }),

                    })
                }),
                
                ///
                ///FOLLOW EARTH PLAYER WHEN ESCORTED
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfEscorted(thisFox),
                    new TaskFollowPlayer(foxAgent, earthPlayer.gameObject, thisFox.shelterWaypoint.transform.position, foxAnimator)
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
                            new CheckIfAnyShelter(thisFox),
                            new CheckIfInRangeOne(thisFox.gameObject, thisFox.shelterWaypoint, 20),
                            new taskInitiatePathTo(foxAgent, thisFox.shelterWaypoint.transform, foxAnimator),
                            new TaskHide(thisFox)
                        }),
                        //Tall grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.grassSet, 20),
                            new CheckForClosestGrass(thisFox, thisFox.grassSet, 20),
                            new TaskLocateClosestGrass(thisFox, foxAgent),
                            new taskInitiatePathToGrass(foxAgent, foxAnimator),
                            new TaskHide(thisFox)
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
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.playerSet, 15),
                            new CheckIfAnimating(foxAnimator),
                            new CheckForClosestPlayer(thisFox, 15),
                            new Timer(3f, new TaskVocalizePlayer(foxAgent, thisFox, 15))
                        }),
                        /*
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfHungry(thisFox),
                            //If hungry, is there anything to eat?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyFood(thisFox),
                                    new CheckIfInRangeAll(thisFox.gameObject, thisFox.buildSet, 5),
                                    new CheckForClosestFood(thisFox, 50),
                                    new taskInitiatePathToFood(foxAgent, foxAnimator),
                                    new Timer(2f, new TaskInitiateAnimation(foxAnimator.animator, foxAnimator.IfEatingHash, nonEatingAnimations)),
                                    new TaskRestoreStat(thisFox.hunger)
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyFood(thisFox)),
                                    new CheckForClosestPlayer(thisFox, 15),
                                    new Timer(3f, new TaskVocalize(foxAgent, thisFox, 15))
                                })
                            })
                        }),
                        */
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisFox),
                            //If thirsty, is there anything to drink?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyWater(thisFox),
                                    new taskInitiatePathTo(foxAgent, thisFox.waterWaypoint.transform, foxAnimator),
                                    new Timer(2f, new TaskInitiateAnimation(foxAnimator.animator, foxAnimator.IfEatingHash, nonEatingAnimations)),
                                    new TaskRestoreStat(thisFox.thirst)
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisFox)),
                                    new CheckForClosestPlayer(thisFox, 15),
                                    new Timer(3f, new TaskVocalize(foxAgent, thisFox, 15, thisFox.waterImage))
                                })
                                
                            })
                            
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisFox),
                            //Find some nice behaviours for a bored duck
                            new TaskRestoreStat(thisFox.entertained)
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
