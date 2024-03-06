using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Two Progress", menuName = "ManagerObject/LevelProgress/LevelTwo")]
public class LevelTwoProgress : LevelProgress
{
    //Start of game condition, saved duck, destroyed oil spill
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

    int treeGoal = 5;
    int grassGoal = 7;
    int cattailGoal = 5;

    public override bool EvaluateFood()
    {
        treeCount = 0;
        grassCount = 0;
        cattailCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Grass")
            {
                grassCount++;
            }
            if (plant.GetComponent<Plant>().stats.plantName == "Tree")
            {
                treeCount++;
            }
            if (plant.GetComponent<Plant>().stats.plantName == "Cattail")
            {
                cattailCount++;
            }
        }

        if (treeCount >= treeGoal && grassCount >= grassGoal && cattailCount >= cattailGoal)
        {
            return true;
        }
        return false;
    }

    protected override void EvaluateLevelProgress()
    {
        if (animalHasEnoughFood && animalHasEnoughWater && animalHasShelter && animalIsSafe)
        {
            OnPlayerWin();
        }
    }

    protected override void OnPlayerWin()
    {

    }

    protected override void OnPlayerLoss()
    {

    }
}
