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

    [Header("These variables set themselves")]
    public NavMeshAgent navAgent;
    public AnimalAnimator animalAnimator;
    public EarthPlayer earthPlayer;
    public CelestialPlayer celestialPlayer;
    public GameObject closestGrass;
    public GameObject closestFood;
    public GameObject closestPlayer;

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

    [SerializeField] protected LevelProgress levelProgress;
    public bool hasCleanWater = false;
    public bool hasShelter = false;
    public bool hasAnyFood = false;

    protected WaitForSeconds barrierLength = new WaitForSeconds(5f);

    protected abstract IEnumerator UpdateAnimalState();

    public abstract bool GetHungryState();
    public abstract bool GetThirstyState();
    public abstract bool GetBoredState();

    public abstract void IsHealed();

    public abstract void ApplyBarrier();
}
