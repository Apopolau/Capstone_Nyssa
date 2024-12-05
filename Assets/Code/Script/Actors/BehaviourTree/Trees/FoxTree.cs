using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class FoxTree : BTree
{
    Fox thisFox;
    NavMeshAgent foxAgent;
    WeatherState weatherState;
    OurAnimator foxAnimator;
    EarthPlayer earthPlayer;

    protected override BTNode SetupTree()
    {
        thisFox = GetComponent<Fox>();
        foxAgent = GetComponent<NavMeshAgent>();
        earthPlayer = thisFox.GetEarthPlayer();
        weatherState = thisFox.GetWeatherManager();
        foxAnimator = thisFox.GetAnimator();

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
                    new CheckIfKidnapped(thisFox),
                    new TaskGetKidnapped(thisFox)
                }),

                ///
                ///STAY IN PLACE IF STUCK
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfStuck(thisFox),
                    new TaskAwaitDeath(thisFox)
                }),

                ///
                ///RUN AND HIDE FROM ENEMIES WHEN IN RANGE
                ///
                new Sequence(new List<BTNode>
                {
                    //Check if enemy nearby
                    new CheckIfInRangeAll(thisFox.gameObject, thisFox.GetEnemySet(), 45),
                    new TaskRunAwayFromTarget(thisFox, thisFox.GetEnemySet(), 45)
                }),
                
                ///
                ///FOLLOW EARTH PLAYER WHEN ESCORTED
                ///
                new Sequence(new List<BTNode>
                {
                    new CheckIfEscorted(thisFox),
                    new TaskFollowPlayer(thisFox, earthPlayer.gameObject, thisFox.GetShelterWaypoint().transform.position)
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
                            new CheckIfInRangeOne(thisFox.gameObject, thisFox.GetShelterWaypoint(), 75),
                            new taskInitiatePathTo(thisFox, thisFox.GetShelterWaypoint()),
                            new TaskHide(thisFox)
                        }),
                        //Tall grass
                        new Sequence(new List<BTNode>
                        {
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.GetGrassSet(), 75),
                            new CheckForClosestGrass(thisFox, thisFox.GetGrassSet(), 75),
                            new TaskLocateClosestGrass(thisFox, foxAgent),
                            new taskInitiatePathToGrass(foxAgent),
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
                            new Inverter(new CheckIfVocalizeOnCooldown(thisFox, "acknowledge")),
                            new CheckIfInRangeAll(thisFox.gameObject, thisFox.GetPlayerSet(), 20),
                            new CheckForClosestPlayer(thisFox, 15),
                            new Timer(3f, new TaskVocalizePlayer(foxAgent, thisFox, 20)),
                            new TaskSetVocalizeOnCooldown(thisFox, "acknowledge")

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
                            new Inverter(new CheckIfVocalizeOnCooldown(thisFox, "thirsty")),
                            new CheckIfThirsty(thisFox),
                            //If thirsty, is there anything to drink?
                            new Selector(new List<BTNode>
                            {
                                new Sequence(new List<BTNode>
                                {
                                    new CheckIfAnyWater(thisFox),
                                    new Sequence(new List<BTNode>
                                    {
                                        //If we're right on top of water, drink
                                        new CheckIfInRangeOne(thisFox.gameObject, thisFox.GetWaterWaypoint(), 5),
                                        //new Timer(2f, new TaskInitiateAnimation(foxAnimator, "eat")),
                                        new TaskRestoreStat(thisFox.GetThirst())
                                    }),
                                    new Sequence(new List<BTNode>
                                    {
                                        new CheckIfInRangeOne(thisFox.gameObject, thisFox.GetWaterWaypoint(), 200),
                                        new taskInitiatePathTo(thisFox, thisFox.GetWaterWaypoint()),
                                    })
                                }),
                                //If no, complain about it
                                new Sequence(new List<BTNode>
                                {
                                    new Inverter(new CheckIfAnyWater(thisFox)),
                                    new CheckForClosestPlayer(thisFox, 20),
                                    new Timer(3f, new TaskVocalize(foxAgent, thisFox, 20, thisFox.GetVocalizeImage("waterImage"))),
                                    new TaskSetVocalizeOnCooldown(thisFox, "thirsty")
                                })
                            })
                        }),
                        //Check if bored finally, play
                        new Sequence(new List<BTNode>
                        {
                            //We could introduce slightly more customized behaviour for each animal with some variations
                            new CheckIfBored(thisFox),
                            new TaskPickRandomWaypoint(thisFox),
                            new TaskPathToWaypoint(thisFox),
                            new Timer(10f, new TaskRestoreStat(thisFox.GetBoredom()))
                        })
                    })
                })
            })
        }) ;
        return root;
        
    }
}
