using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreeProgress : LevelProgress
{
    bool hasSavedFox;

    bool hasOpenedBridge1;
    bool hasOpenedBridge2;

    bool hasCleanedWater;

    bool hasSavedRabbit;

    public bool animalHasEnoughFood = false;
    //public bool animalHasEnoughWater = false;
    //public bool animalHasShelter = false;
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
        if (animalHasEnoughFood && cleanWater && shelter && animalIsSafe)
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
