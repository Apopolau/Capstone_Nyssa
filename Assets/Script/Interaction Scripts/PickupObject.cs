using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : Interactable
{
    protected SpriteRenderer spriteRenderer;

    [SerializeField] Item item;
    [SerializeField] Inventory inventory;

    //public GameObject boxRange;

    EarthPlayer earthPlayer;

    private void Start()
    {
        if (item != null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = item.Icon;
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            uiObject.SetActive(false);
        }

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
        ItemPickup();
    }

    public void ItemPickup()
    {
        if (isInRange && earthPlayer.interacting)
        {
            //Debug.Log("Picking up");
            if (item != null)
            {
                int pickupQuantity = 1; // You can change this to the desired quantity.
                inventory.AddItem(item, pickupQuantity);
                //item = null;
                //spriteRenderer.enabled = false;
                
                //uiObject.SetActive(false);
                //boxRange.SetActive(false);

                // Add this anywhere where the task should be crossed out when completed.
                //TaskListManager task1 = GameObject.Find("Task1").GetComponent<TaskListManager>();
                //task1.CrossOutTask();

            }

            Destroy(this.GetComponentInParent<Transform>().gameObject);
            //Debug.LogWarning("in range and key pressed");
        }
    }
    

}