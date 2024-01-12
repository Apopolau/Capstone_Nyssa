using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;
    [SerializeField] KeyCode PickupKey = KeyCode.E; //change for controller input

    [SerializeField] SpriteRenderer spriteRenderer;


    private bool isInRange;
    
    private void OnValidate()
    {
        spriteRenderer.sprite = item.Icon;
        spriteRenderer.enabled = true;
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(PickupKey))
        {
            if (item != null)
            {
                 inventory.AddItem(item);
                 item = null;
                spriteRenderer.enabled = false;
        

               
                 
                //increase the count of the item
            }
           
            Debug.LogWarning("in range and key pressed");
           

        }
    }


    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            isInRange = true; 
            spriteRenderer.enabled = true;
            Debug.LogWarning("in range");
        }
       

    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
         
    }

}

