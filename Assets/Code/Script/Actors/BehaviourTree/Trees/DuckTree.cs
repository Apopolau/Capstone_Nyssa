using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class DuckTree : BTree
{
    Duck thisDuck;
    NavMeshAgent duckAgent;
    GameObject managerObject;
    WeatherState weatherState;
    OurAnimator duckAnimator;
    EarthPlayer earthPlayer;
    CelestialPlayer celestialPlayer;

    protected override BTNode SetupTree()
    {
        thisDuck = GetComponent<Duck>();
        duckAgent = GetComponent<NavMeshAgent>();
        earthPlayer = thisDuck.GetEarthPlayer();
        celestialPlayer = thisDuck.GetCelestialPlayer();
        weatherState = thisDuck.GetWeatherManager();
        duckAnimator = thisDuck.GetAnimator();

        /*
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
        */

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
                    new CheckIfStuck(thisDuck),
                    new TaskAwaitDeath(duckAgent)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetEnemySet(), 20),
                    //If there is an enemy, run or hide
                    new Selector(new List<BTNode>
                    {
                        //RUN!
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetEnemySet(), 10),
                            new TaskRunAwayFromTarget(thisDuck, thisDuck.GetEnemySet(), 10)
                        }),
                        //If not in immediate proximity of a monster, prioritize shelter first
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisDuck),
                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetShelterWaypoint(), 20),
                            new taskInitiatePathTo(duckAgent, thisDuck.GetShelterWaypoint().transform, duckAnimator),
                            new TaskHide(thisDuck)
                        }),
                        //If no shelter, find grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetGrassSet(), 20),
                            new CheckForClosestGrass(thisDuck, thisDuck.GetGrassSet(), 20),
                            new TaskLocateClosestGrass(thisDuck, duckAgent),
                            new taskInitiatePathToGrass(duckAgent, duckAnimator),
                            new TaskHide(thisDuck)
                        }),

                    })
                }),
                
                ///
                ///FOLLOW EARTH PLAYER WHEN ESCORTED
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfEscorted(thisDuck),
                    new TaskFollowPlayer(thisDuck, earthPlayer.gameObject, thisDuck.GetShelterWaypoint().transform.position)
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
                            new CheckIfAnyShelter(thisDuck),
                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetShelterWaypoint(), 20),
                            new taskInitiatePathTo(duckAgent, thisDuck.GetShelterWaypoint().transform, duckAnimator),
                            new TaskHide(thisDuck)
                        }),
                        //Tall grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetGrassSet(), 20),
                            new CheckForClosestGrass(thisDuck, thisDuck.GetGrassSet(), 20),
                            new TaskLocateClosestGrass(thisDuck, duckAgent),
                            new taskInitiatePathToGrass(duckAgent, duckAnimator),
                            new TaskHide(thisDuck)
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
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetPlayerSet(), 15),
                            new CheckIfAnimating(duckAnimator),
                            new CheckForClosestPlayer(thisDuck, 15),
                            new Timer(3f, new TaskVocalizePlayer(duckAgent, thisDuck, 15))
                        }),
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfHungry(thisDuck),
                            //If hungry, is there anything to eat?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyFood(thisDuck),
                                    new Selector(new List<BTNode>
                                    {
                                        new Sequence(new List<BTNode>
                                        {
                                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetBuildSet(), 5),
                                            new CheckForClosestFood(thisDuck, 5),
                                            new Timer(2f, new TaskInitiateAnimation(duckAnimator, "eat")),
                                            new TaskRestoreStat(thisDuck.GetHunger())
                                        }),
                                        new Sequence(new List<BTNode>
                                        {
                                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetBuildSet(), 50),
                                            new CheckForClosestFood(thisDuck, 50),
                                            new taskInitiatePathToFood(duckAgent, duckAnimator),
                                        })
                                    }),
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyFood(thisDuck)),
                                    new CheckForClosestPlayer(thisDuck, 15),
                                    new Timer(3f, new TaskVocalize(duckAgent, thisDuck, 15, thisDuck.GetVocalizeImage("foodImage")))
                                })
                            })
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfThirsty(thisDuck),
                            //If thirsty, is there anything to drink?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyWater(thisDuck),
                                    new Selector(new List<BTNode>
                                    {
                                        new Sequence(new List<BTNode>
                                        {
                                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetWaterWaypoint(), 5),
                                            new Timer(2f, new TaskInitiateAnimation(duckAnimator, "eat")),
                                            new TaskRestoreStat(thisDuck.GetThirst())
                                        }),
                                        new Sequence(new List<BTNode>
                                        {
                                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetWaterWaypoint(), 50),
                                            new taskInitiatePathTo(duckAgent, thisDuck.GetWaterWaypoint().transform, duckAnimator),
                                        })
                                    })
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisDuck)),
                                    new CheckForClosestPlayer(thisDuck, 15),
                                    new Timer(3f, new TaskVocalize(duckAgent, thisDuck, 15, thisDuck.GetVocalizeImage("waterImage")))
                                })
                                
                            })
                            
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfBored(thisDuck),
                            //Find some nice behaviours for a bored duck
                            new TaskRestoreStat(thisDuck.GetBoredom())
                        })
                    })
                })
            })
        }); ;
        return root;
        
    }
}
