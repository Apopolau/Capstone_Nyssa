using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Manager Object", menuName = "Manager Object/Level Manager Object")]
public class LevelManagerObject : ScriptableObject
{
    [SerializeField] public LevelEventManager eventManager;
    [SerializeField] LevelProgress levelProgress;

    [SerializeField, Range(1,4)] public int currentLevel;

    public void SetEventManager(LevelEventManager incEventManager)
    {
        eventManager = incEventManager;
    }
}
