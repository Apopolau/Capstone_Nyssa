using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : Animal
{
    private WaitForSeconds degredateRate = new WaitForSeconds(1);
    
    /*
    public Duck(bool stuckness)
    {
        stuck = stuckness;
    }
    */

    private void Awake()
    {
        hunger = new Stat(100, 100, false);
        thirst = new Stat(50, 50, false);
        entertained = new Stat(150, 150, false);
        isScared = false;
        isHiding = false;
        animalAnimator = GetComponentInChildren<AnimalAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
        //levelProgress = managerObject.GetComponent<LevelProgress>();
    }

    private void Start()
    {
        foreach(GameObject player in playerSet.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
            else if(player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
        }
        StartCoroutine(UpdateAnimalState());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimalState();
    }

    override protected IEnumerator UpdateAnimalState()
    {
        while (true)
        {
            yield return degredateRate;
            hunger.current -= Mathf.Clamp(1, 0, hunger.max);
            thirst.current -= Mathf.Clamp(1, 0, thirst.max);
            entertained.current -= Mathf.Clamp(1, 0, entertained.max);

            if (hunger.current <= 25)
            {
                hunger.low = true;
            }
            if (thirst.current <= 25)
            {
                thirst.low = true;
            }
            if (entertained.current <= 25)
            {
                entertained.low = true;
            }
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
        return hunger.low;
    }

    public override bool GetThirstyState()
    {
        return thirst.low;
    }

    public override bool GetBoredState()
    {
        return entertained.low;
    }

    //We can put something here to change their texture and such
    public override void IsHealed()
    {
        
    }

    public override void ApplyBarrier()
    {
        isShielded = true;
        StartCoroutine(BarrierWearsOff());
    }

    private IEnumerator BarrierWearsOff()
    {
        yield return barrierLength;
        isShielded = false;
    }
}
