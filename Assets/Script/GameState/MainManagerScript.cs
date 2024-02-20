using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerScript : MonoBehaviour
{
    [SerializeField] LevelManagerObject levelManager;
    [SerializeField] EventManager eventManager;

    private void Awake()
    {
        eventManager = GetComponent<EventManager>();
        levelManager.SetEventManager(eventManager);
    }
}
