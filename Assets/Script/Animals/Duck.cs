using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : Animal
{
    public Duck(bool stuckness)
    {
        stuck = stuckness;
    }

    private void Awake()
    {
        hunger = new Stat(100, 100, false);
        thirst = new Stat(50, 50, false);
        entertained = new Stat(150, 150, false);
        scared = false;
        hiding = false;
        animalAnimator = GetComponentInChildren<AnimalAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
        levelProgress = managerObject.GetComponent<LevelProgress>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimalState();
    }

    override protected void UpdateAnimalState()
    {
        hunger.current--;
        hunger.current = Mathf.Clamp(hunger.current, 0, hunger.max);
        thirst.current--;
        thirst.current = Mathf.Clamp(thirst.current, 0, thirst.max);
        entertained.current--;
        entertained.current = Mathf.Clamp(entertained.current, 0, entertained.max);

        if(hunger.current <= 25)
        {
            hunger.low = true;
        }
        if (thirst.current <= 25)
        {
            thirst.low = true;
        }
        if(entertained.current <= 25)
        {
            entertained.low = true;
        }
    }

    protected void CheckLevelState()
    {
        if(levelProgress.totalPlants > 0)
        {
            hasAnyFood = true;
        }
        else
        {
            hasAnyFood = false;
        }

        if (levelProgress.cleanWater)
        {
            hasCleanWater = true;
        }
        else
        {
            hasCleanWater = false;
        }

        if (levelProgress.shelter)
        {
            hasShelter = true;
        }
        else
        {
            hasShelter = false;
        }
    }

    public override bool GetHungryState()
    {
        return hunger;
    }

    public override bool GetThirstyState()
    {
        return thirst;
    }

    public override bool GetBoredState()
    {
        return entertained;
    }
}
