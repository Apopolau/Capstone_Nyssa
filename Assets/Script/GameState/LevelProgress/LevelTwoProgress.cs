using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Two Progress", menuName = "ManagerObject/LevelProgress/LevelTwo")]
public class LevelTwoProgress : LevelProgress
{
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

    public bool animalHasEnoughFood = false;
    public bool animalHasEnoughWater = false;
    public bool animalHasShelter = false;
    public bool animalIsSafe = false;

    public override bool EvaluateLevelProgress()
    {
        if (animalHasEnoughFood && cleanWater && animalIsSafe)
        {
            OnAllObjectivesComplete();
            return true;
        }
        return false;
    }

    public override bool EvaluateFood()
    {
        if (EvaluateTrees() && EvaluateGrass() && EvaluateCattails() && EvaluateFlowers() && EvaluateLilies())
        {
            animalHasEnoughFood = true;
            return true;
        }
        return false;
    }

    protected override void OnAllObjectivesComplete()
    {

    }

    protected override void OnPlayerWin()
    {

    }

    protected override void OnPlayerLoss()
    {

    }
}
