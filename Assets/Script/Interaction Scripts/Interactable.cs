using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObjectRuntimeSet playerSet;
    protected EarthPlayer earthPlayer;
    protected CelestialPlayer celestialPlayer;

    [SerializeField] protected HUDManager hudManager;

    [SerializeField] protected bool p1IsInRange = false;
    [SerializeField] protected bool isEarthInteractable = false;
    [SerializeField] protected bool p2IsInRange = false;
    [SerializeField] protected bool isCelestialInteractable = false;
    public GameObject uiObject;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUIElement();
    }

    protected void UpdateUIElement()
    {
        if((p1IsInRange && isEarthInteractable) || (p2IsInRange && isCelestialInteractable))
        {
            uiObject.SetActive(true);
        }
        else if((!isEarthInteractable && !p2IsInRange) || (!isCelestialInteractable && !p1IsInRange))
        {
            uiObject.SetActive(false);
        }
        else if(!p1IsInRange && !p2IsInRange)
        {
            uiObject.SetActive(false);
        }
        
    }
}
