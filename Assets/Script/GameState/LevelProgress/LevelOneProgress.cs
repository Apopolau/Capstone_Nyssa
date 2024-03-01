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
    public bool hasTurnedOffPump;
    public bool animalHasEnoughFood = false;
    public bool animalIsSafe = false;

    int treeGoal = 5;
    int grassGoal = 7;
    int cattailGoal = 5;

    //Drag this from the plant prefabs folder
    [SerializeField]public GameObject treeSeedPrefab;
    [SerializeField] public GameObject grassSeedPrefab;


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

        totalPlants = grassCount + treeCount + cattailCount;

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
