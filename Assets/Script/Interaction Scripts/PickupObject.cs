using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : Interactable
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;

    public GameObject boxRange;

    EarthPlayer earthPlayer;

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

    private void Start()
    {
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
        ItemPickup();
    }

    public void ItemPickup()
    {
        if (isInRange && earthPlayer.interacting)
        {
            if (item != null)
            {
                int pickupQuantity = 1; // You can change this to the desired quantity.
                inventory.AddItem(item, pickupQuantity);
                item = null;
                spriteRenderer.enabled = false;
                
                uiObject.SetActive(false);
                boxRange.SetActive(false);

            }

            Debug.LogWarning("in range and key pressed");
        }
    }
    

}