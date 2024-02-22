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

    [Header("These can be set on the prefab")]
    [SerializeField] public  GameObjectRuntimeSet playerSet;
    [SerializeField] public  GameObjectRuntimeSet enemySet;
    [SerializeField] public  GameObjectRuntimeSet buildSet;
    public WeatherState weatherState;

    protected bool stuck;
    public bool midAnimation;
    public Stat hunger;
    public Stat thirst;
    public Stat entertained;
    protected bool scared;
    protected bool hiding;

    [SerializeField] protected LevelProgress levelProgress;
    public bool hasCleanWater = false;
    public bool hasShelter = false;
    public bool hasAnyFood = false;

    protected abstract IEnumerator UpdateAnimalState();

    public abstract bool GetHungryState();
    public abstract bool GetThirstyState();
    public abstract bool GetBoredState();
}
