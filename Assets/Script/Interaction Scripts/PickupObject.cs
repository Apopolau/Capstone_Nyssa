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
            spriteRenderer.sprite = item.stats.Icon;
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
                if (inventory.AddItem(item, item.quantity))
                {
                    UpdateUIText();
                    Destroy(item);
                    Destroy(this.GetComponentInParent<Transform>().gameObject);
                }
                else {
                    earthPlayer.displayText.text = "Inventory is full";
                }
            }
        }
    }

    private void UpdateUIText()
    {
        // Find all InventoryUITextUpdater scripts in the scene and call UpdateQuantityText() on each of them
        PlantingUIIndicator[] textUpdaters = FindObjectsOfType<PlantingUIIndicator>();
        foreach (PlantingUIIndicator textUpdater in textUpdaters)
        {
            textUpdater.UpdateQuantityText();
        }
    }

    //Call this when instantiating an item prefab to set the quantity of items being dropped
    public void SetItemQuantity(int newQuantity)
    {
        item.quantity = newQuantity;
    }
    

}