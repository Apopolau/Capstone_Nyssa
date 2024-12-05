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
    

    [Header("These variables set themselves")]
    //protected NavMeshAgent navAgent;
    protected EarthPlayer earthPlayer;
    protected CelestialPlayer celestialPlayer;
    
    [SerializeField] protected GameObject kidnapIcon;
    [SerializeField] protected GameObject closestGrass;
    [SerializeField] protected GameObject closestFood;
    [SerializeField] protected GameObject closestPlayer;

    [Header("These can be set on the prefab")]
    [SerializeField] protected LevelProgress levelProgress;
    [SerializeField] protected WeatherState weatherState;
    [SerializeField] protected UserSettingsManager userSettingsManager;
    [SerializeField] protected AnimalSoundLibrary soundLibrary;
    [SerializeField] protected GameObject uiTarget;
    [SerializeField] protected GameObject escortPopup;
    [SerializeField] protected GameObject speechBubble;
    [SerializeField] protected InteractPrompt interactPrompt;
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

    [Header("Stats")]
    protected Stat hunger;
    protected Stat thirst;
    protected Stat lonely;
    protected Stat entertained;

    protected Dictionary<string, bool> needOnCooldown;
    protected bool hungryOnCooldown = false;
    protected bool thirstyOnCooldown = false;
    protected bool unsafeOnCooldown = false;
    protected bool lonelyOnCooldown = false;
    protected bool acknowledgeOnCooldown = false;

    [Header("Behaviour states")]
    [SerializeField] protected bool isStuck;
    //[SerializeField] protected bool isScared = false;
    [SerializeField] protected bool isHiding = false;
    [SerializeField] protected bool isShielded = false;
    [SerializeField] protected bool isEscorted = false;
    protected bool resettingEscort = false;
    [SerializeField] protected bool isKidnapped = false;

    [Header("Other Kidnap Variables")]
    [SerializeField] protected Sprite kidnapIconSprite;
    [SerializeField] protected KidnappingEnemy kidnapper;
    [SerializeField] protected bool kidnapIconOn;

    [Header("Other variables")]
    [SerializeField] protected bool hasCleanWater = false;
    [SerializeField] protected bool hasShelter = false;
    [SerializeField] protected bool hasAnyFood = false;
    [SerializeField] protected bool midAnimation = false;
    [SerializeField] protected bool inRangeOfEscort = false;

    //Timers
    protected WaitForSeconds barrierLength = new WaitForSeconds(5f);
    protected WaitForSeconds popupLength = new WaitForSeconds(3);
    protected WaitForSeconds resetTimer = new WaitForSeconds(0.5f);
    protected WaitForSeconds behaviourCooldown = new WaitForSeconds(15f);

    virtual protected void Awake()
    {
        needOnCooldown = new Dictionary<string, bool>();
        needOnCooldown.Add("hungry", hungryOnCooldown);
        needOnCooldown.Add("thirsty", thirstyOnCooldown);
        needOnCooldown.Add("unsafe", unsafeOnCooldown);
        needOnCooldown.Add("lonely", lonelyOnCooldown);
        needOnCooldown.Add("acknowledge", acknowledgeOnCooldown);

        wanderWaypoints = new List<GameObject>();
        wanderWaypoints.Add(shelterWaypoint);
        wanderWaypoints.Add(waterWaypoint);

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

    /*
    public bool GetIsScared()
    {
        return isScared;
    }

    public void SetScared(bool scared)
    {
        isScared = scared;
    }
    */

    public void SetWalkingState()
    {
        if (GetComponent<NavMeshAgent>().enabled && GetComponent<NavMeshAgent>().hasPath)
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
        if (status)
        {
            soundLibrary.PlayPanicClips();
            GetComponent<CapsuleCollider>().enabled = false;
            ResetAgentPath();
            GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
        }
        isKidnapped = status;
        UpdateKidnapIcon();
    }

    public void UpdateKidnapIcon()
    {
        if (isKidnapped)
        {
            kidnapIconOn = true;
            kidnapIcon.SetActive(true);
            HandleKidnapIcon();
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
            managerObject.GetComponent<HUDManager>().MoveKidnapIcon(kidnapIcon, uiTarget);
        }
    }

    protected void FollowKidnapper()
    {
        this.transform.position = kidnapper.GetKidnapAnimalTarget().transform.position;
    }

    /*
    public void SetKidnapSpeed()
    {
        GetNavMeshAgent().speed = kidnappedSpeed;
    }
    */

    /*
    public void ResetOrigSpeed()
    {
        GetNavMeshAgent().speed = origSpeed;
    }
    */

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
        return agent;
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

    protected IEnumerator ResetEscort()
    {
        resettingEscort = true;
        yield return resetTimer;
        resettingEscort = false;
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
        //Get the sprite we're modifying
        speechBubble.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;

        UpdateSpriteScale(speechBubble.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());

        speechBubble.SetActive(true);
        StartCoroutine(PopupRoutine());
    }

    public IEnumerator PopupRoutine()
    {
        yield return popupLength;
        speechBubble.SetActive(false);
    }

    public IEnumerator RunVocalizeCooldown(string cooldownName)
    {
        //Debug.Log(this.gameObject + " starting " + cooldownName + " cooldown");
        needOnCooldown[cooldownName] = true;
        yield return behaviourCooldown;
        needOnCooldown[cooldownName] = false;
        //Debug.Log(this.gameObject + " ending " + cooldownName + " cooldown");
    }

    public bool GetIsNeedOnCooldown(string cooldownName)
    {
        return needOnCooldown[cooldownName];
    }

    public void ToggleEscortPopup(bool on)
    {
        escortPopup.SetActive(on);
        interactPrompt.SetButtonPrompt(earthPlayer);
    }

    void UpdateSpriteScale(SpriteRenderer _sprite)
    {
        float xWidth = 0.7f / _sprite.sprite.bounds.size.x;
        float yWidth = 0.7f / _sprite.sprite.bounds.size.y;
        _sprite.transform.localScale = new Vector3(xWidth, yWidth, 1);
    }
}
