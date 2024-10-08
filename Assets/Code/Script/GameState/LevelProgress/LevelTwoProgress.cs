using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Two Progress", menuName = "Data Type/Level Progress/Level Two")]
public class LevelTwoProgress : LevelProgress
{
    LevelEventManager levelTwoEvents;
    [SerializeField] public GameObject flowerSeedPrefab;

    //Start of game condition, saved hedgehog, destroyed monster
    bool hasSavedHedgehog;
    //Defeated the monster on the short path
    bool hasFlowerSeeds;
    //Cleared the rock slide

    //Defeated the monster up the path
    bool cleanedSidePath;
    //Cleaned the water at the top of the path
    bool hasShutOffDevice;



    protected override void OnAllObjectivesComplete()
    {

    }

    protected override void OnPlayerWin()
    {

    }

    protected override void OnPlayerLoss()
    {

    }

    public override void SetPowers(bool active)
    {
        hasMoonTide = active;
    }

    public bool GetMoonTideStatus()
    {
        return hasMoonTide;
    }

    public override void SetEventManager(LevelEventManager eventManager)
    {
        levelTwoEvents = eventManager;
    }

    public override LevelEventManager GetEventManager()
    {
        return levelTwoEvents;
    }
}
