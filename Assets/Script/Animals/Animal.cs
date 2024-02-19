using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    protected DayNightCycle dayNightCycle;
    protected GameObject shelterWaypoint;
    protected GameObject waterWaypoint;

    protected NavMeshAgent navAgent;
    protected AnimalAnimator animalAnimator;

    protected bool hungry;
    protected bool thirsty;
    protected bool bored;
    protected bool scared;
    protected bool hiding;

    protected int hungerMax;
    protected int hungerCurrent;
    protected int thirstMax;
    protected int thirstCurrent;
    protected int boredMax;
    protected int boredCurrent;

    protected GameObjectRuntimeSet playerSet;
    protected GameObjectRuntimeSet enemySet;
    protected GameObjectRuntimeSet buildSet;

    protected abstract void UpdateAnimalState();

    public abstract bool GetHungryState();
    public abstract bool GetThirstyState();
    public abstract bool GetBoredState();
}
