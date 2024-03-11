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
    protected int flowerCount;
    protected int lilyCount;
    public int totalPlants;
    public bool cleanWater = false;
    public bool shelter = false;


    public void SetObjectiveText(TextMeshProUGUI incObjectiveText)
    {
        objectiveText = incObjectiveText;
    }

    public abstract bool EvaluateFood();
    public abstract bool EvaluateLevelProgress();

    protected abstract void OnAllObjectivesComplete();

    protected abstract void OnPlayerWin();

    protected abstract void OnPlayerLoss();

}
