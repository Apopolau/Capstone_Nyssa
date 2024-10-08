using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObjectRuntimeSet playerSet;
    protected EarthPlayer earthPlayer;
    protected CelestialPlayer celestialPlayer;

    [SerializeField] protected HUDManager hudManager;

    [Tooltip("Whether or not Sprout should be able to interact with this object")]
    [SerializeField] protected bool isEarthInteractable = false;
    [Tooltip("Don't manually set this")]
    [SerializeField] protected bool p1IsInRange = false;
    [Tooltip("Whether or not Celeste should be able to interact with this object")]
    [SerializeField] protected bool isCelestialInteractable = false;
    [Tooltip("Don't manually set this")]
    [SerializeField] protected bool p2IsInRange = false;

    [Tooltip("The object attached to this one that holds the root for the ui popup")]
    [SerializeField] protected GameObject uiObject;

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
