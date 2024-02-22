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
                if (inventory.AddItem(item, pickupQuantity))
                {
                    Destroy(this.GetComponentInParent<Transform>().gameObject);
                }
                else {
                    earthPlayer.displayText.text = "Inventory is full";
                };
            }
        }
    }

    public void SetItemQuantity(int newQuantity)
    {
        item.Quantity = newQuantity;
    }
    

}