using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hedgehog : Animal
{
    private WaitForSeconds degredateRate = new WaitForSeconds(1);



    protected override void Awake()
    {
        base.Awake();

        hunger = new Stat(50, 200, false);
        thirst = new Stat(50, 200, false);
        entertained = new Stat(50, 100, false);
        animator = GetComponentInChildren<HedgehogAnimator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        foreach (GameObject player in playerSet.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
            else if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateAnimalState());
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelState();
        SetWalkingState();
        HandleKidnapIcon();
        if (earthPlayer.GetIsInteracting() && inRangeOfEscort)
        {
            if (!isEscorted)
            {
                SetEscort(true);
            }
            else if (isEscorted)
            {
                SetEscort(false);
            }
        }
        if(isEscorted && escortPopup.activeSelf)
        {
            ToggleEscortPopup(false);
        }
        if (isKidnapped)
        {
            FollowKidnapper();
        }
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

        if (levelProgress.animalHasWater)
        {
            hasCleanWater = true;
        }
        else
        {
            hasCleanWater = false;
        }

        if (levelProgress.animalHasShelter)
        {
            hasShelter = true;
        }
        else
        {
            hasShelter = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!weatherState.dayTime)
        {
            if (other.GetComponent<EarthPlayer>() && !isEscorted)
            {
                ToggleEscortPopup(true);
                inRangeOfEscort = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (escortPopup.activeSelf && other.GetComponent<EarthPlayer>())
        {
            ToggleEscortPopup(false);
            inRangeOfEscort = false;
        }
    }

    //We can put something here to change their texture and such
    public override void IsHealed()
    {
        
    }
}
