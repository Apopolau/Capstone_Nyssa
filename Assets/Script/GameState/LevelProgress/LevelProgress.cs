using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class LevelProgress : ScriptableObject
{
    [SerializeField] protected GameObjectRuntimeSet plantSet;
    [SerializeField] protected TextMeshProUGUI objectiveText;
    [SerializeField] protected Inventory inventory;

    protected int currentPlayerLevel;

    [SerializeField] protected int treeGoal;
    [SerializeField] protected int grassGoal;
    [SerializeField] protected int cattailGoal;
    [SerializeField] protected int flowerGoal;
    [SerializeField] protected int lilyGoal;

    [SerializeField] protected int treeCount;
    [SerializeField] protected int grassCount;
    [SerializeField] protected int cattailCount;
    [SerializeField] protected int flowerCount;
    [SerializeField] protected int lilyCount;

    public int totalPlants;
    public bool animalHasEnoughFood = false;
    public bool animalIsSafe = false;
    public bool animalHasFriend = false;
    public bool animalHasWater = false;
    public bool animalHasShelter = false;
    public bool readyToLeave = false;


    private void OnDisable()
    {
        animalHasEnoughFood = false;
        animalIsSafe = false;
        animalHasFriend = false;
        animalHasWater = false;
        animalHasShelter = false;
        readyToLeave = false;
    }

    public void SetObjectiveText(TextMeshProUGUI incObjectiveText)
    {
        objectiveText = incObjectiveText;
    }

    //public abstract bool EvaluateFood();
    //public abstract bool EvaluateLevelProgress();


    /// <summary>
    /// FUNCTIONS FOR GETTING FOOD AMOUNTS
    /// </summary>
    /// <returns></returns>
    /// 
    public int GetTotalPlantCount()
    {
        return plantSet.Items.Count;
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

    public int GetTreeCount()
    {
        return treeCount;
    }

    public int GetTreeGoal()
    {
        return treeGoal;
    }

    public int GetGrassCount()
    {
        return grassCount;
    }

    public int GetGrassGoal()
    {
        return grassGoal;
    }

    public int GetCattailCount()
    {
        return cattailCount;
    }

    public int GetCattailGoal()
    {
        return cattailGoal;
    }

    public int GetFlowerCount()
    {
        return flowerCount;
    }

    public int GetFlowerGoal()
    {
        return flowerGoal;
    }

    public int GetLilyCount()
    {
        return lilyCount;
    }

    public int GetLilyGoal()
    {
        return lilyGoal;
    }


    /// <summary>
    /// FUNCTIONS FOR MEASURING LEVEL COMPLETENESS
    /// </summary>
    public bool GetFoodStatus()
    {
        return animalHasEnoughFood;
    }

    public bool GetWaterStatus()
    {
        return animalHasWater;
    }

    public bool GetSafetyStatus()
    {
        return animalIsSafe;
    }

    public bool GetFriendStatus()
    {
        return animalHasFriend;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    ///
    ///FUNCTIONS FOR WRAPPING UP THE LEVEL
    ///
    protected abstract void OnAllObjectivesComplete();

    protected abstract void OnPlayerWin();

    protected abstract void OnPlayerLoss();

}
