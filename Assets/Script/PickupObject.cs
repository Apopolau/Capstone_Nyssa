using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;
    [SerializeField] KeyCode PickupKey = KeyCode.E; //change for controller input

    [SerializeField] SpriteRenderer spriteRenderer;

    public GameObject boxRange;

    public GameObject uiObject;

    private bool isInRange;
    
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

    private void Update()
    {
        ItemPickup();
    }

    public void ItemPickup()
    {
        if (isInRange && Input.GetKeyDown(PickupKey))
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
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player1")
        {
            isInRange = true;
            uiObject.SetActive(true);
            

            // Check if the item is not null before enabling the spriteRenderer
            if (item != null)
            {
                spriteRenderer.enabled = true;
                
            }

            Debug.LogWarning("in range");
        }
       

    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        uiObject.SetActive(false);
         
    }

}