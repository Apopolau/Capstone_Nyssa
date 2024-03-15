using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Three Progress", menuName = "ManagerObject/LevelProgress/LevelThree")]
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
    int flowerGoal = 5;
    int lilyGoal = 8;

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

    public bool EvaluateTrees()
    {
        treeCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Tree")
            {
                treeCount++;
            }
        }

        if (treeCount >= treeGoal)
        {
            return true;
        }
        return false;
    }

    public bool EvaluateGrass()
    {
        grassCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Grass")
            {
                grassCount++;
            }
        }

        if (grassCount >= grassGoal)
        {
            return true;
        }
        return false;
    }

    public bool EvaluateCattails()
    {
        cattailCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Cattail")
            {
                cattailCount++;
            }
        }

        if (cattailCount >= cattailGoal)
        {
            return true;
        }
        return false;
    }

    public bool EvaluateFlowers()
    {
        flowerCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Flower")
            {
                flowerCount++;
            }
        }

        if (flowerCount >= flowerGoal)
        {
            return true;
        }
        return false;
    }

    public bool EvaluateLilies()
    {
        lilyCount = 0;

        foreach (GameObject plant in plantSet.Items)
        {
            if (plant.GetComponent<Plant>().stats.plantName == "Lily")
            {
                lilyCount++;
            }
        }

        if (lilyCount >= lilyGoal)
        {
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

    public int GetTreeCount()
    {
        return treeCount;
    }

    public int GetGrassCount()
    {
        return grassCount;
    }

    public int GetCattailCount()
    {
        return cattailCount;
    }

    public int GetFlowerCount()
    {
        return flowerCount;
    }

    public int GetLilyCount()
    {
        return lilyCount;
    }
}
