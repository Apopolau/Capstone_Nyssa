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


        BTNode root = new Selector(new List<BTNode>
        {
            //Pick our appropriate tree based on time of day
            new Selector(new List<BTNode>
            {
                ///
                /// GO WITH ENEMY IF KIDNAPPED
                /// 
                 new Sequence(new List<BTNode>
                {
                    new CheckIfKidnapped(thisDuck),
                    new TaskGetKidnapped(thisDuck)
                }),

                ///
                ///STAY IN PLACE IF STUCK
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfStuck(thisDuck),
                    new TaskAwaitDeath(thisDuck)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetEnemySet(), 50),
                    new TaskRunAwayFromTarget(thisDuck, thisDuck.GetEnemySet(), 50)
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
                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetShelterWaypoint(), 100),
                            new taskInitiatePathTo(thisDuck, thisDuck.GetShelterWaypoint()),
                            new TaskPathToWaypoint(thisDuck),
                            new TaskHide(thisDuck)
                        }),
                        //Tall grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetGrassSet(), 150),
                            new CheckForClosestGrass(thisDuck, thisDuck.GetGrassSet(), 150),
                            new TaskLocateClosestGrass(thisDuck, duckAgent),
                            new taskInitiatePathToGrass(duckAgent),
                            new TaskPathToWaypoint(thisDuck),
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
                            new Inverter(new CheckIfVocalizeOnCooldown(thisDuck, "acknowledge")),
                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetPlayerSet(), 20),
                            new CheckForClosestPlayer(thisDuck, 15),
                            new Timer(3f, new TaskVocalizePlayer(duckAgent, thisDuck, 20)),
                            new TaskSetVocalizeOnCooldown(thisDuck, "acknowledge")
                        }),
                        //Check if hungry first, find food
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfVocalizeOnCooldown(thisDuck, "hungry")),
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
                                            new CheckIfInRangeAll(thisDuck.gameObject, thisDuck.GetBuildSet(), 200),
                                            new CheckForClosestFood(thisDuck, 200),
                                            new taskInitiatePathToFood(thisDuck),
                                            new TaskPathToWaypoint(thisDuck)
                                        })
                                    }),
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyFood(thisDuck)),
                                    new CheckForClosestPlayer(thisDuck, 20),
                                    new Timer(3f, new TaskVocalize(duckAgent, thisDuck, 20, thisDuck.GetVocalizeImage("foodImage"))),
                                    new TaskSetVocalizeOnCooldown(thisDuck, "hungry")
                                })
                            })
                        }),
                        //Check if thirsty next, find water
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfVocalizeOnCooldown(thisDuck, "thirsty")),
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
                                            new CheckIfInRangeOne(thisDuck.gameObject, thisDuck.GetWaterWaypoint(), 200),
                                            new taskInitiatePathTo(thisDuck, thisDuck.GetWaterWaypoint()),
                                            new TaskPathToWaypoint(thisDuck)
                                        })
                                    })
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisDuck)),
                                    new CheckForClosestPlayer(thisDuck, 20),
                                    new Timer(3f, new TaskVocalize(duckAgent, thisDuck, 20, thisDuck.GetVocalizeImage("waterImage"))),
                                    new TaskSetVocalizeOnCooldown(thisDuck, "thirsty")
                                })

                            })

                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            //We could introduce slightly more customized behaviour for each animal with some variations
                            new CheckIfBored(thisDuck),
                            new TaskPickRandomWaypoint(thisDuck),
                            new TaskPathToWaypoint(thisDuck),
                            new Timer(10f, new TaskRestoreStat(thisDuck.GetBoredom()))
                        })
                    })
                })
            })
        });
        return root;
        
    }
}
