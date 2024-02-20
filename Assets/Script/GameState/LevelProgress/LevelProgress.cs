using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class LevelProgress : ScriptableObject
{
    [SerializeField] protected GameObjectRuntimeSet plantSet;
    [SerializeField] protected TextMeshProUGUI objectiveText;

    protected int currentPlayerLevel;

    protected int treeCount;
    protected int grassCount;
    protected int cattailCount;
    public int totalPlants;
    public bool cleanWater = false;
    public bool shelter = false;


    public abstract bool EvaluateFood();
    protected abstract void EvaluateLevelProgress();

    protected abstract void OnPlayerWin();

    protected abstract void OnPlayerLoss();
}
