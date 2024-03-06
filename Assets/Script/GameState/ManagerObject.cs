using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Manager Object", menuName = "ManagerObject/LevelManagerObject")]
public class LevelManagerObject : ScriptableObject
{
    [SerializeField] EventManager eventManager;
    [SerializeField] LevelProgress levelProgress;

    [SerializeField, Range(1,4)] public int currentLevel;

    public void SetEventManager(EventManager incEventManager)
    {
        eventManager = incEventManager;
    }
}
