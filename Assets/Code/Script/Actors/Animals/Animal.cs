using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Animal : Actor
{
    [Header("Set this from the scene")]
    [SerializeField] protected GameObject managerObject;
    [SerializeField] protected GameObject shelterWaypoint;
    [SerializeField] protected GameObject waterWaypoint;
    [SerializeField] protected GameObject kidnapIcon;

    [Header("These variables set themselves")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected EarthPlayer earthPlayer;
    [SerializeField] protected CelestialPlayer celestialPlayer;
    [SerializeField] public GameObject closestGrass;
    [SerializeField] public GameObject closestFood;
    [SerializeField] public GameObject closestPlayer;

    [Header("These can be set on the prefab")]
    [SerializeField] protected LevelProgress levelProgress;
    [SerializeField] protected WeatherState weatherState;
    [SerializeField] protected UserSettingsManager userSettingsManager;
    [SerializeField] protected AnimalSoundLibrary soundLibrary;
    [SerializeField] protected GameObject uiTarget;
    [SerializeField] protected GameObject escortPopup;
    [SerializeField] protected InteractPrompt interactPrompt;
    [SerializeField] protected GameObject speechTarget;
    [SerializeField] protected GameObjectRuntimeSet playerSet;
    [SerializeField] protected GameObjectRuntimeSet enemySet;
    [SerializeField] protected GameObjectRuntimeSet buildSet;
    [SerializeField] protected GameObjectRuntimeSet foodSet;
    [SerializeField] protected GameObjectRuntimeSet grassSet;

    protected Dictionary<string, Sprite> vocalizeSprites;
    [SerializeField] protected Sprite waterImage;
    [SerializeField] protected Sprite foodImage;
    [SerializeField] protected Sprite friendImage;
    [SerializeField] protected Sprite sproutImage;
    [SerializeField] protected Sprite celesteImage;
    [SerializeField] protected Sprite kidnapIconSprite;

    [Header("Stats")]
    protected Stat hunger;
    protected Stat thirst;
    protected Stat entertained;

    [Header("Behaviour states")]
    protected bool isStuck;
    protected bool isScared;
    protected bool isHiding = false;
    protected bool isShielded = false;
    protected bool isEscorted = false;
    protected bool isKidnapped = false;

    [Header("Other Kidnap Variables")]
    protected KidnappingEnemy kidnapper;
    [SerializeField] protected float origSpeed;
    [SerializeField] protected float kidnappedSpeed;
    protected bool kidnapIconOn;

    [Header("Other variables")]
    protected bool hasCleanWater = false;
    protected bool hasShelter = false;
    protected bool hasAnyFood = false;
    protected bool midAnimation = false;
    protected bool inRangeOfEscort = false;

    //Timers
    protected WaitForSeconds barrierLength = new WaitForSeconds(5f);
    protected WaitForSeconds popupLength = new WaitForSeconds(3);

    virtual protected void Awake()
    {
        vocalizeSprites = new Dictionary<string, Sprite>();
        vocalizeSprites.Add("waterImage", waterImage);
        vocalizeSprites.Add("foodImage", foodImage);
        vocalizeSprites.Add("friendImage", friendImage);
        vocalizeSprites.Add("sproutImage", sproutImage);
        vocalizeSprites.Add("celesteImage", celesteImage);
    }

    /// <summary>
    /// STATE HELPERS
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator UpdateAnimalState();

    public void Unstuck()
    {
        isStuck = false;
    }

    public bool GetIsStuck()
    {
        return isStuck;
    }

    public void SetStuck(bool stuck)
    {
        isStuck = stuck;
    }

    public bool GetHungryState()
    {
        return hunger.low;
    }

    public Stat GetHunger()
    {
        return hunger;
    }

    public bool GetThirstyState()
    {
        return thirst.low;
    }

    public Stat GetThirst()
    {
        return thirst;
    }

    public bool GetBoredState()
    {
        return entertained.low;
    }

    public Stat GetBoredom()
    {
        return entertained;
    }

    public bool GetIsScared()
    {
        return isScared;
    }

    public void SetWalkingState()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > new Vector3(0.1f, 0.1f, 0.1f).magnitude)
        {
            animator.SetAnimationFlag("move", true);
        }
        else if (GetComponent<NavMeshAgent>().enabled && GetComponent<NavMeshAgent>().hasPath)
        {
            animator.SetAnimationFlag("move", true);
        }
        else
        {
            animator.SetAnimationFlag("move", false);
        }
    }

    

    /// <summary>
    /// KIDNAP FUNCTIONS
    /// </summary>
    public bool GetIsKidnapped()
    {
        return isKidnapped;
    }

    public void SetKidnapStatus(bool status)
    {
        isKidnapped = status;
        UpdateKidnapIcon();
        if (status)
        {
            soundLibrary.PlayPanicClips();
            SetKidnapSpeed();
        }
        else
        {
            ResetOrigSpeed();
        }
    }

    public void UpdateKidnapIcon()
    {
        Debug.Log("Updating kidnap icon");
        if (isKidnapped)
        {
            kidnapIconOn = true;
            kidnapIcon.SetActive(true);
        }
        else
        {
            kidnapIconOn = false;
            kidnapIcon.SetActive(false);
        }
    }

    protected void HandleKidnapIcon()
    {
        if (kidnapIconOn)
        {
            /*
            kidnapIcon.transform.position = uiTarget.transform.position;
            float xPos = Mathf.Clamp(uiTarget.transform.position.x, 0f, Screen.width);
            float yPos = Mathf.Clamp(uiTarget.transform.position.x, 0f, Screen.height);
            kidnapIcon.transform.position = new Vector3(xPos, yPos, 0);

            Vector3 pointToRotateTowards = new Vector3(0, 0, kidnapIcon.transform.GetChild(0).GetChild(1).transform.position.z);

            kidnapIcon.transform.GetChild(0).GetChild(0).LookAt(this.gameObject.transform);
            */
            managerObject.GetComponent<HUDManager>().MoveKidnapIcon(kidnapIcon, uiTarget, this.gameObject);
        }
    }

    public void SetKidnapSpeed()
    {
        GetNavMeshAgent().speed = kidnappedSpeed;
    }

    public void ResetOrigSpeed()
    {
        GetNavMeshAgent().speed = origSpeed;
    }

    public KidnappingEnemy GetKidnapper()
    {
        return kidnapper;
    }

    public void SetKidnapper(KidnappingEnemy enemy)
    {
        kidnapper = enemy;
    }

    public void SetKidnapIconObject(GameObject kidnapIconObject)
    {
        kidnapIcon = kidnapIconObject;
    }

    public void SetKidnapIconImage(Sprite kidnapIconImage)
    {
        kidnapIconSprite = kidnapIconImage;
    }

    public Sprite GetKidnapIcon()
    {
        return kidnapIconSprite;
    }

    public GameObjectRuntimeSet GetEnemySet()
    {
        return enemySet;
    }



    /// <summary>
    /// GENERAL GETTERS AND SETTERS
    /// </summary>
    /// <returns></returns>

    public WeatherState GetWeatherManager()
    {
        return weatherState;
    }

    public GameObjectRuntimeSet GetPlayerSet()
    {
        return playerSet;
    }

    public GameObject GetClosestPlayer()
    {
        return closestPlayer;
    }

    public void SetClosestPlayer(GameObject newPlayer)
    {
        closestPlayer = newPlayer;
    }

    public EarthPlayer GetEarthPlayer()
    {
        return earthPlayer;
    }

    public CelestialPlayer GetCelestialPlayer()
    {
        return celestialPlayer;
    }

    public GameObjectRuntimeSet GetFoodSet()
    {
        return foodSet;
    }

    public GameObjectRuntimeSet GetGrassSet()
    {
        return grassSet;
    }

    public GameObjectRuntimeSet GetBuildSet()
    {
        return buildSet;
    }

    public GameObject GetClosestFood()
    {
        return closestFood;
    }

    public void SetClosestFood(GameObject newFood)
    {
        closestFood = newFood;
    }

    public GameObject GetClosestGrass()
    {
        return closestGrass;
    }

    public void SetClosestGrass(GameObject newGrass)
    {
        closestGrass = newGrass;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navAgent;
    }

    public GameObject GetShelterWaypoint()
    {
        return shelterWaypoint;
    }

    public GameObject GetWaterWaypoint()
    {
        return waterWaypoint;
    }

    public bool GetIsEscorted()
    {
        return isEscorted;
    }

    public void SetEscort(bool status)
    {
        isEscorted = status;
    }

    public bool GetIsHiding()
    {
        return isHiding;
    }

    public void SetIsHiding(bool hiding)
    {
        isHiding = hiding;
    }

    public bool GetHasShelter()
    {
        return hasShelter;
    }

    public bool GetHasFood()
    {
        return hasAnyFood;
    }

    public bool GetHasWater()
    {
        return hasCleanWater;
    }

    

    //Used if we heal the animal
    public abstract void IsHealed();

    public void ApplyBarrier()
    {
        isShielded = true;
        StartCoroutine(BarrierWearsOff());
    }

    protected IEnumerator BarrierWearsOff()
    {
        yield return barrierLength;
        isShielded = false;
    }

    public void PlayVocal()
    {
        soundLibrary.PlayVocalizeClips();
    }

    public Sprite GetVocalizeImage(string spriteName)
    {
        return vocalizeSprites[spriteName];
    }

    public void TurnOnSpeechPopup(Sprite sprite)
    {
        speechTarget.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
        speechTarget.SetActive(true);
        StartCoroutine(PopupRoutine());
    }

    public IEnumerator PopupRoutine()
    {
        yield return popupLength;
        speechTarget.SetActive(false);
    }

    public void ToggleEscortPopup(bool on)
    {
        escortPopup.SetActive(on);
        interactPrompt.SetButtonPrompt(earthPlayer);
    }

    
}
