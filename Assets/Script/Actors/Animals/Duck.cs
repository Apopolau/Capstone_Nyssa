using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : Animal
{
    private WaitForSeconds degredateRate = new WaitForSeconds(1);

    //[SerializeField] DialogueCameraPan panToDuck;

    //Used to determine where duck will randomly wander
    [Header("Duck Random Pathing Points")]
    [SerializeField] public List<GameObject> duckWayPoints;
    //Corresponds to waypoints
    bool CanGoToUpperArea;
    bool CanGoToFarBank;
    bool CanGoToHalfwayPoint;
    bool CanGoToTopArea;

    private void Awake()
    {
        hunger = new Stat(100, 100, false);
        thirst = new Stat(50, 50, false);
        entertained = new Stat(150, 150, false);
        isScared = false;
        isHiding = false;
        animalAnimator = GetComponentInChildren<AnimalAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
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

    private void Update()
    {
        CheckLevelState();
        SetWalkingState();
        MoveKidnapIcon();
        if (earthPlayer.GetIsInteracting() && inRangeOfEscort)
        {
            if (isEscorted)
            {
                SetEscort(true);
            }
            else if (!isEscorted)
            {
                SetEscort(false);
            }
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
            if (other.GetComponent<EarthPlayer>())
            {
                uiTarget.SetActive(true);
                inRangeOfEscort = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (uiTarget.activeSelf && other.GetComponent<EarthPlayer>())
        {
            uiTarget.SetActive(false);
            inRangeOfEscort = false;
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

    //Helpers
    public void SetUpperBankOn()
    {
        CanGoToUpperArea = true;
    }

    public void SetFarBankOn()
    {
        CanGoToFarBank = true;
    }

    public void SetHalfwayPointOn()
    {
        CanGoToHalfwayPoint = true;
    }

    public void SetTopAreaOn()
    {
        CanGoToTopArea = true;
    }
}
