using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    [Header("Set this from the scene")]
    public GameObject managerObject;
    public GameObject shelterWaypoint;
    public GameObject waterWaypoint;
    public GameObject kidnapIcon;

    [Header("These variables set themselves")]
    [SerializeField] protected NavMeshAgent navAgent;
    [SerializeField] protected AnimalAnimator animalAnimator;
    [SerializeField] protected EarthPlayer earthPlayer;
    [SerializeField] protected CelestialPlayer celestialPlayer;
    [SerializeField] public GameObject closestGrass;
    [SerializeField] public GameObject closestFood;
    [SerializeField] public GameObject closestPlayer;

    [Header("These can be set on the prefab")]
    [SerializeField] public GameObjectRuntimeSet playerSet;
    [SerializeField] public GameObjectRuntimeSet enemySet;
    [SerializeField] public GameObjectRuntimeSet buildSet;
    [SerializeField] public GameObjectRuntimeSet foodSet;
    [SerializeField] public GameObjectRuntimeSet grassSet;

    [SerializeField] public Sprite waterImage;
    [SerializeField] public Sprite foodImage;
    [SerializeField] public Sprite shelterImage;
    [SerializeField] public Sprite friendImage;
    [SerializeField] public Sprite sproutImage;
    [SerializeField] public Sprite celesteImage;
    public WeatherState weatherState;
    public GameObject uiTarget;

    public bool isStuck;
    public bool midAnimation;
    public Stat hunger;
    public Stat thirst;
    public Stat entertained;
    public bool isScared;
    public bool isHiding;
    public bool isShielded;
    public bool isEscorted;
    public bool isKidnapped;
    public Enemy kidnapper;
    private bool kidnapIconOn;
    protected bool inRangeOfEscort;

    public float origSpeed;
    public float kidnappedSpeed;

    [SerializeField] protected LevelProgress levelProgress;
    public bool hasCleanWater = false;
    public bool hasShelter = false;
    public bool hasAnyFood = false;

    protected WaitForSeconds barrierLength = new WaitForSeconds(5f);

    

    protected abstract IEnumerator UpdateAnimalState();

    /// <summary>
    /// STATE HELPERS
    /// </summary>
    /// <returns></returns>
    public abstract bool GetHungryState();
    public abstract bool GetThirstyState();
    public abstract bool GetBoredState();

    public void SetWalkingState()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > new Vector3(0.1f, 0.1f, 0.1f).magnitude)
        {
            GetComponent<AnimalAnimator>().animator.SetBool(GetComponent<AnimalAnimator>().IfWalkingHash, true);
        }
        else if (GetComponent<NavMeshAgent>().hasPath)
        {
            GetComponent<AnimalAnimator>().animator.SetBool(GetComponent<AnimalAnimator>().IfWalkingHash, true);
        }
        else if (!GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<AnimalAnimator>().animator.SetBool(GetComponent<AnimalAnimator>().IfWalkingHash, false);
        }
        else
        {
            GetComponent<AnimalAnimator>().animator.SetBool(GetComponent<AnimalAnimator>().IfWalkingHash, false);
        }
    }

    public void SetCurrSpeed()
    {
         origSpeed=GetNavMeshAgent().speed;
    }
    public void ResetOrigSpeed()
    {
         GetNavMeshAgent().speed=origSpeed;
    }


    public void Unstuck()
    {
        isStuck = false;
    }

    /// <summary>
    /// REFERENCE HELPERS
    /// </summary>
    /// <returns></returns>
    public EarthPlayer GetEarthPlayer()
    {
        return earthPlayer;
    }

    public CelestialPlayer GetCelestialPlayer()
    {
        return celestialPlayer;
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

    public AnimalAnimator GetAnimator()
    {
        return animalAnimator;
    }

    public GameObject GetClosestPlayer()
    {
        return closestPlayer;
    }

    public void SetClosestPlayer(GameObject newPlayer)
    {
        closestPlayer = newPlayer;
    }

    public void SetEscort(bool status)
    {
        Debug.Log("Animal is being escorted: " + status);
        isEscorted = status;
    }

    /// <summary>
    /// POWER HELPERS
    /// </summary>

    public abstract void IsHealed();

    public abstract void ApplyBarrier();

    public void UpdateKidnapIcon()
    {
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

    protected void MoveKidnapIcon()
    {
        if (kidnapIconOn)
        {
            kidnapIcon.transform.position = uiTarget.transform.position;
            float xPos = Mathf.Clamp(uiTarget.transform.position.x, 0f, Screen.width);
            float yPos = Mathf.Clamp(uiTarget.transform.position.x, 0f, Screen.height);
            kidnapIcon.transform.position = new Vector3(xPos, yPos, 0);

            //kidnapIcon.transform.GetChild(0).GetChild(0).RotateAround(this.gameObject.transform.position, kidnapIcon.transform.GetChild(0).GetChild(1).transform.position, 5 * Time.deltaTime);
        }
    }
}
