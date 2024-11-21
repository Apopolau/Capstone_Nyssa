using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : Animal
{
    private WaitForSeconds degredateRate = new WaitForSeconds(1);

    //Used to determine where duck will randomly wander
    [Header("Duck Random Pathing Points")]
    [SerializeField] public List<GameObject> duckWayPoints;
    //Corresponds to waypoints
    //private bool CanGoToUpperArea;
    //private bool CanGoToFarBank;
    //private bool CanGoToHalfwayPoint;
    //private bool CanGoToTopArea;

    

    protected override void Awake()
    {
        base.Awake();

        hunger = new Stat(100, 100, false);
        thirst = new Stat(50, 50, false);
        entertained = new Stat(150, 150, false);
        isScared = false;
        isHiding = false;
        animator = GetComponent<DuckAnimator>();
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
        if (isEscorted && escortPopup.activeSelf)
        {
            escortPopup.SetActive(false);
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



    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>


    /*
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
    */
}
