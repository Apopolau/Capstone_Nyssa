using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsUI : Interactable
{
    [SerializeField] MeshRenderer m_Renderer;
    //[SerializeField] Item item;
    //[SerializeField] Inventory inventory;
    [SerializeField] LevelOneEvents levelOneEvents;


    private void Update()
    {
        TurnOff();
    }

    private void Start()
    {
        foreach (GameObject player in playerSet.Items)
        {
            if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
            else if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
        }
    }

    public void TurnOff()
    {
        if (p1IsInRange && earthPlayer.interacting)
        {
    
            uiObject.SetActive(false);
        }
    }
}
