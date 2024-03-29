using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerScript : MonoBehaviour
{
    [SerializeField] public LevelManagerObject levelManager;
    [SerializeField] LevelEventManager eventManager;

    private void Awake()
    {
        eventManager = GetComponent<LevelEventManager>();
        levelManager.SetEventManager(eventManager);
    }
}
