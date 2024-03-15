using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class LevelProgress : ScriptableObject
{
    [SerializeField] protected GameObjectRuntimeSet plantSet;
    [SerializeField] protected TextMeshProUGUI objectiveText;

    protected int currentPlayerLevel;

    [SerializeField] protected int treeGoal;
    [SerializeField] protected int grassGoal;
    [SerializeField] protected int cattailGoal;
    [SerializeField] protected int flowerGoal;
    [SerializeField] protected int lilyGoal;

    protected int treeCount;
    protected int grassCount;
    protected int cattailCount;
    protected int flowerCount;
    protected int lilyCount;

    public int totalPlants;
    public bool cleanWater = false;
    public bool shelter = false;
    public bool readyToLeave = false;


    public void SetObjectiveText(TextMeshProUGUI incObjectiveText)
    {
        objectiveText = incObjectiveText;
    }

    public abstract bool EvaluateFood();
    public abstract bool EvaluateLevelProgress();

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

    protected abstract void OnAllObjectivesComplete();

    protected abstract void OnPlayerWin();

    protected abstract void OnPlayerLoss();

}
