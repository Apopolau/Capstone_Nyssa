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
    [SerializeField] public TextMeshProUGUI pickupLetter; // Reference to the TextMeshPro component

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

        GameObject textObject = GameObject.Find("Letter"); 
        if (textObject != null)
        {
            pickupLetter = textObject.GetComponent<TextMeshProUGUI>();
            if (pickupLetter == null)
            {
                Debug.LogError("TextMeshPro component not found on the object named: " + textObject.name);
            }
        }

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
            //Debug.Log("Picking up");
            if (item != null)
            {
                if (inventory.AddItem(item, item.quantity))
                {
                    UpdateUIText();
                    Destroy(this.GetComponentInParent<Transform>().gameObject);
                }
                else {
                    StartCoroutine(ThrowInventoryWarning());
                }
            }
        }
    }

    private IEnumerator ThrowInventoryWarning()
    {
        earthPlayer.displayText.text = "Inventory is full";
        yield return inventoryDisplay;
        earthPlayer.displayText.text = "";
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
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = true;
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
        // Update the text based on the control type from the user settings manager
        if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.KEYBOARD)
        {
            pickupLetter.text = "P"; //only changing one letter
        } 
        else if (userSettingsManager.earthControlType == UserSettingsManager.ControlType.CONTROLLER)
        {
            pickupLetter.text = "A"; 
        }
        
    } 
}