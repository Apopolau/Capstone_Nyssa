using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : Animal
{
    [Header("Set this from the scene")]
    public new DayNightCycle dayNightCycle;
    public new GameObject shelterWaypoint;
    public new GameObject waterWaypoint;

    [Header("These variables set themselves")]
    public new NavMeshAgent navAgent;
    public new AnimalAnimator animalAnimator;

    public new bool hungry;
    public new bool thirsty;
    public new bool bored;
    public new bool scared;
    public new bool hiding;

    private new int hungerMax = 100;
    private new int hungerCurrent = 100;
    private new int thirstMax = 50;
    private new int thirstCurrent = 50;
    private new int boredMax = 150;
    private new int boredCurrent = 150;

    [Header("These can be set on the prefab")]
    [SerializeField] public new GameObjectRuntimeSet playerSet;
    [SerializeField] public new GameObjectRuntimeSet enemySet;
    [SerializeField] public new GameObjectRuntimeSet buildSet;

    private void Awake()
    {
        animalAnimator = GetComponentInChildren<AnimalAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimalState();
    }

    override protected void UpdateAnimalState()
    {
        hungerCurrent--;
        hungerCurrent = Mathf.Clamp(hungerCurrent, 0, hungerMax);
        thirstCurrent--;
        thirstCurrent = Mathf.Clamp(thirstCurrent, 0, thirstMax);
        boredCurrent--;
        boredCurrent = Mathf.Clamp(boredCurrent, 0, boredMax);

        if(hungerCurrent <= 25)
        {
            hungry = true;
        }
        if (thirstCurrent <= 25)
        {
            thirsty = true;
        }
        if(boredCurrent <= 25)
        {
            bored = true;
        }
    }

    public override bool GetHungryState()
    {
        return hungry;
    }

    public override bool GetThirstyState()
    {
        return thirsty;
    }

    public override bool GetBoredState()
    {
        return bored;
    }
}
