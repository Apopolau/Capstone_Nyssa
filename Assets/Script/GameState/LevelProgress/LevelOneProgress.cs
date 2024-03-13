using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level One Progress", menuName = "ManagerObject/LevelProgress/LevelOne")]
public class LevelOneProgress : LevelProgress
{
    
    //Start of game condition, saved duck, destroyed oil spill
    bool hasSavedDuck = false;
    //Defeated the monster on the short path
    bool hasTallGrass = false;
    //Cleared the rock slide
    bool clearedRockSlide = false;
    //Defeated the monster up the path
    public bool cleanedLongPath;
    //Cleaned the water at the top of the path
    public bool hasTurnedOffPump = false;
    public bool animalHasEnoughFood = false;
    public bool animalIsSafe = false;
    public bool animalHasFriend = false;

    int treeGoal = 10;
    int grassGoal = 15;
    int cattailGoal = 8;

    //Drag this from the plant prefabs folder
    [SerializeField]public GameObject treeSeedPrefab;
    [SerializeField] public GameObject grassSeedPrefab;


    public override bool EvaluateFood()
    {
        if (EvaluateTrees() && EvaluateGrass() && EvaluateCattails())
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

    public override bool EvaluateLevelProgress()
    {
        if (animalHasEnoughFood && cleanWater && animalHasFriend && animalIsSafe)
        {
            OnAllObjectivesComplete();
            return true;
        }
        return false;
    }

    protected override void OnAllObjectivesComplete()
    {
        readyToLeave = true;
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

}
