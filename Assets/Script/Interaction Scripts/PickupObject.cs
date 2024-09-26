using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupObject : Interactable
{
    protected SpriteRenderer spriteRenderer;

    [SerializeField] Item item;
    [SerializeField] ItemStats itemStats;
    [SerializeField] Inventory inventory;
    [SerializeField] public int quantity;

    //public EarthUIManager earthUIManager;

    public bool isBeingAbsorbed = false;

    WaitForSeconds absorbDelay = new WaitForSeconds(0.1f);
    WaitForSeconds inventoryDisplay = new WaitForSeconds(3);

    public UserSettingsManager userSettingsManager;
    [SerializeField] GameObject pickupLetter; // Reference to the TextMeshPro component

    //[SerializeField] GameObjectRuntimeSet playerSet;

    private void Awake()
    {
        isEarthInteractable = true;
        isCelestialInteractable = true;
    }

    private void Start()
    {
        item = new Item(itemStats, quantity);

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

        // Access the user settings manager
        //userSettingsManager = earthUIManager.userSettingsManager;

        //GameObject textObject = GameObject.Find("Letter"); 
        

        UpdatePickupLetter();
    }

    private void Update()
    {
        ItemPickup();
        UpdateUIElement();
        UpdatePickupLetter();
    }

    public void ItemPickup()
    {
        if ((p1IsInRange && earthPlayer.interacting) || (p2IsInRange && celestialPlayer.interacting) )
        {
            if (item != null)
            {
                if (inventory.AddItem(item, item.quantity))
                {
                    UpdateUIText();
                    Destroy(this.GetComponentInParent<Transform>().gameObject);
                }
                else {
                    ThrowInventoryWarning();
                }
            }
        }
    }

    private void ThrowInventoryWarning()
    {
        string warningText = "Inventory is full";
        hudManager.ThrowPlayerWarning(warningText);
    }

    private void UpdateUIText()
    {
        // Find all InventoryUITextUpdater scripts in the scene and call UpdateQuantityText() on each of them
        PlantingUIIndicator[] textUpdaters = FindObjectsOfType<PlantingUIIndicator>();
        foreach (PlantingUIIndicator textUpdater in textUpdaters)
        {
            textUpdater.UpdateQuantityTextOnce();
        }
    }

    //Call this when instantiating an item prefab to set the quantity of items being dropped
    public void SetItemQuantity(int newQuantity)
    {
        quantity = newQuantity;
        if(item != null)
        {
            item.quantity = quantity;
        }
    }

    public void SetInventory(Inventory newInventory)
    {
        inventory = newInventory;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = true;

            // Update the text based on the control type from the user settings manager
            if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
            {
                pickupLetter.GetComponent<TextMeshProUGUI>().text = "P";
            } 
            else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
            pickupLetter.GetComponent<TextMeshProUGUI>().text = "A";
            }
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = true;

            // Update pickup letter based on celestial player's control type
            if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.KEYBOARD)
            {
                pickupLetter.GetComponent<TextMeshProUGUI>().text = "E";
            } 
            else if (userSettingsManager.celestialControlType == UserSettingsManager.ControlType.CONTROLLER)
            {
                pickupLetter.GetComponent<TextMeshProUGUI>().text = "A";
            }
        }

        // If one player goes out of range, update pickup letter back to "P" if EarthPlayer is still in range
        if (!p2IsInRange && p1IsInRange && userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            pickupLetter.GetComponent<TextMeshProUGUI>().text = "P";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = false;

        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = false;

        }
    }

   private void UpdatePickupLetter()
    {
        
        
    } 
}