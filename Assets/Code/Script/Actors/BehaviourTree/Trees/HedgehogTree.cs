using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class HedgehogTree : BTree
{
    Hedgehog thisHog;
    NavMeshAgent hogAgent;
    WeatherState weatherState;
    OurAnimator hogAnimator;
    EarthPlayer earthPlayer;

    protected override BTNode SetupTree()
    {
        thisHog = GetComponent<Hedgehog>();
        hogAgent = GetComponent<NavMeshAgent>();
        earthPlayer = thisHog.GetEarthPlayer();
        weatherState = thisHog.GetWeatherManager();
        hogAnimator = thisHog.GetAnimator();

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
                    new CheckIfKidnapped(thisHog),
                    new TaskGetKidnapped(thisHog)
                }),

                ///
                ///STAY IN PLACE IF STUCK
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfStuck(thisHog),
                    new TaskAwaitDeath(thisHog)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisHog.gameObject, thisHog.GetEnemySet(), 50),
                    new TaskRunAwayFromTarget(thisHog, thisHog.GetEnemySet(), 50)
                }),
                
                ///
                ///FOLLOW EARTH PLAYER WHEN ESCORTED
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfEscorted(thisHog),
                    new TaskFollowPlayer(thisHog, earthPlayer.gameObject, thisHog.GetShelterWaypoint().transform.position)
                }),

                ///
                /// IF NONE OF THOSE THINGS, FOLLOW THESE BEHAVIOURS BY DEFAULT
                /// NIGHT TIME ///
                /// 
                new Sequence(new List<BTNode>
                {
                    new CheckIfNight(weatherState),
                    //Pick between hiding in the shelter and hiding in tall grass
                    new Selector(new List<BTNode>
                    {
                        //FIND SHELTER SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfAnyShelter(thisHog),
                            new CheckIfInRangeOne(thisHog.gameObject, thisHog.GetShelterWaypoint(), 100),
                            new taskInitiatePathTo(thisHog, thisHog.GetShelterWaypoint()),
                            new TaskPathToWaypoint(thisHog),
                            new TaskHide(thisHog)
                        }),

                        //FIND TALL GRASS SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.GetGrassSet(), 150),
                            new CheckForClosestGrass(thisHog, thisHog.GetGrassSet(), 150),
                            new taskInitiatePathToGrass(hogAgent),
                            new TaskPathToWaypoint(thisHog),
                            new TaskHide(thisHog)
                        }),
                    })
                }),


                ///
                /// IF NONE OF THOSE THINGS, FOLLOW THESE BEHAVIOURS BY DEFAULT
                /// DAY TIME ///
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
                            //// RECOGNIZE PLAYER SEQUENCE
                            new Inverter(new CheckIfVocalizeOnCooldown(thisHog, "acknowledge")),
                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.GetPlayerSet(), 20),
                            new CheckForClosestPlayer(thisHog, 15),
                            new Timer(3f, new TaskVocalizePlayer(hogAgent, thisHog, 20)),
                            new TaskSetVocalizeOnCooldown(thisHog, "acknowledge")
                        }),

                        //ALLEVIATE HUNGER SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new Inverter(new CheckIfVocalizeOnCooldown(thisHog, "hungry")),
                            new CheckIfHungry(thisHog),
                            //If hungry, is there anything to eat?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyFood(thisHog),
                                    new Selector(new List<BTNode>
                                    {
                                        new Sequence(new List<BTNode>
                                        {
                                            //If we're on top of a source of food, eat
                                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.GetBuildSet(), 5),
                                            new CheckForClosestFood(thisHog, 5),
                                            new Timer(2f, new TaskInitiateAnimation(hogAnimator, "eat")),
                                            new TaskRestoreStat(thisHog.GetHunger())
                                        }),
                                        new Sequence(new List<BTNode>
                                        {
                                            //Otherwise, try to find the closest source
                                            new CheckIfInRangeAll(thisHog.gameObject, thisHog.GetBuildSet(), 200),
                                            new CheckForClosestFood(thisHog, 200),
                                            new taskInitiatePathToFood(thisHog),
                                            new TaskPathToWaypoint(thisHog)
                                        })
                                    })
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyFood(thisHog)),
                                    new CheckForClosestPlayer(thisHog, 20),
                                    new Timer(3f, new TaskVocalize(hogAgent, thisHog, 20, thisHog.GetVocalizeImage("foodImage"))),
                                    new TaskSetVocalizeOnCooldown(thisHog, "hungry")
                                })
                            })
                        }),

                        //// ALLEVIATE THIRST SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfVocalizeOnCooldown(thisHog, "thirsty"),
                            new CheckIfThirsty(thisHog),
                            //If thirsty, is there anything to drink?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyWater(thisHog),
                                    new Selector(new List<BTNode>
                                    {
                                        new Sequence(new List<BTNode>
                                        {
                                            //If we're right on top of water, drink
                                            new CheckIfInRangeOne(thisHog.gameObject, thisHog.GetWaterWaypoint(), 5),
                                            new Timer(2f, new TaskInitiateAnimation(hogAnimator, "eat")),
                                            new TaskRestoreStat(thisHog.GetThirst())
                                        }),
                                        new Sequence(new List<BTNode>
                                        {
                                            new CheckIfInRangeOne(thisHog.gameObject, thisHog.GetWaterWaypoint(), 200),
                                            new taskInitiatePathTo(thisHog, thisHog.GetWaterWaypoint()),
                                            new TaskPathToWaypoint(thisHog)
                                        })
                                    })
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisHog)),
                                    new CheckForClosestPlayer(thisHog, 20),
                                    new Timer(3f, new TaskVocalize(hogAgent, thisHog, 20, thisHog.GetVocalizeImage("waterImage"))),
                                    new TaskSetVocalizeOnCooldown(thisHog, "thirsty")
                                })

                            })

                        }),

                        //// ALLEVIATE BOREDOM SEQUENCE
                        new Sequence(new List<BTNode>
                        {
                            //We could introduce slightly more customized behaviour for each animal with some variations
                            new CheckIfBored(thisHog),
                            new TaskPickRandomWaypoint(thisHog),
                            new TaskPathToWaypoint(thisHog),
                            new Timer(10f, new TaskRestoreStat(thisHog.GetBoredom()))
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
