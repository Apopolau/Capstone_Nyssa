using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Nyssa : Animal
{
    private bool inRangeOfPickup = false;
    private bool beingHeld = false;
    private WaitForSeconds degredateRate = new WaitForSeconds(1);
    [SerializeField] private GameObject uiText;

    //private Dictionary<string, bool> animationFlags;

    protected override void Awake()
    {
        base.Awake();

        hunger = new Stat(100, 100, false);
        thirst = new Stat(50, 50, false);
        entertained = new Stat(150, 150, false);
        isScared = false;
        isHiding = false;
        animator = GetComponentInChildren<OurAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        
    }

    private void Start()
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
        StartCoroutine(UpdateAnimalState());
    }

    private void Update()
    {
        CheckLevelState();
        SetWalkingState();
        HandleKidnapIcon();
        if (earthPlayer.GetIsInteracting() && inRangeOfPickup)
        {
            if (!beingHeld)
            {
                SetPickup();
            }
        }
        else if(!earthPlayer.GetIsHoldingNyssa() && beingHeld)
        {
            UnSetPickup();
        }
        StaysWithSprout();
    }

    override protected IEnumerator UpdateAnimalState()
    {

        while (true)
        {
            yield return degredateRate;
            /*
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
            */
        }

    }

    protected void CheckLevelState()
    {
        if (levelProgress.totalPlants > 0)
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

        if (other.GetComponent<EarthPlayer>())
        {
            uiTarget.SetActive(true);
            inRangeOfPickup = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (uiTarget.activeSelf && other.GetComponent<EarthPlayer>())
        {
            uiTarget.SetActive(false);
            inRangeOfPickup = false;
        }
    }

    public void SetPickup()
    {
        earthPlayer.PickUpNyssa();
        SphereCollider[] colliders = GetComponents<SphereCollider>();
        foreach(SphereCollider collider in colliders)
        {
            collider.enabled = false;
        }
        if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            uiText.GetComponent<TextMeshProUGUI>().text = "Put down";
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            uiText.GetComponent<TextMeshProUGUI>().text = "Déposer";
        }
        
        beingHeld = true;
    }

    public void UnSetPickup()
    {
        SphereCollider[] colliders = GetComponents<SphereCollider>();
        foreach (SphereCollider collider in colliders)
        {
            collider.enabled = true;
        }
        beingHeld = false;
        if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            uiText.GetComponent<TextMeshProUGUI>().text = "Pick up";
        }
        else if (userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            uiText.GetComponent<TextMeshProUGUI>().text = "Ramasser";
        }
        

        this.transform.position = earthPlayer.transform.position + earthPlayer.transform.forward * 5;
    }

    public void StaysWithSprout()
    {
        if (beingHeld)
        {
            this.transform.position = earthPlayer.GetNyssaTarget().transform.position;
        }
    }

    //We can put something here to change their texture and such
    public override void IsHealed()
    {

    }

}
