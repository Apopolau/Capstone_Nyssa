using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] protected GameObjectRuntimeSet plantSet;
    [SerializeField] protected TextMeshProUGUI objectiveText;

    int currentPlayerLevel;

    public bool animalHasEnoughFood = false;
    public bool animalHasEnoughWater = false;
    public bool animalHasShelter = false;
    public bool animalIsSafe = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void EvaluateLevelProgress()
    {
        if(animalHasEnoughFood && animalHasEnoughWater && animalHasShelter && animalIsSafe)
        {
            OnPlayerWin();
        }
    }

    private void OnPlayerWin()
    {

    }

    private void OnPlayerLoss()
    {

    }
}
