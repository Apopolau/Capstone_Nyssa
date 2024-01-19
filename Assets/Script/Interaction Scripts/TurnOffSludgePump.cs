using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSludgePump : Interactable
{
    [SerializeField] MeshRenderer m_Renderer;
    //[SerializeField] Item item;
    //[SerializeField] Inventory inventory;
    [SerializeField] LevelOneEvents levelOneEvents;

    public GameObject boxRange;

    EarthPlayer earthPlayer;

    /*
    private void OnValidate()
    {

        if (item != null)
        {
            spriteRenderer.sprite = item.Icon;
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            uiObject.SetActive(false);
        }
    }
    */

    void Start()
    {
        players = playerRuntimeSet.Items;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<EarthPlayer>())
            {
                earthPlayer = player.GetComponent<EarthPlayer>();
            }
        }
    }

    private void Update()
    {
        TurnOff();
    }

    public void TurnOff()
    {
        if (isInRange && earthPlayer.interacting)
        {
            levelOneEvents.OnPumpShutOff();
            uiObject.SetActive(false);
            boxRange.SetActive(false);
        }
    }
}
