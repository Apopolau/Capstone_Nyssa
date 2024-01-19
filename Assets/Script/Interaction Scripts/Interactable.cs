using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool isInRange;
    public GameObject uiObject;
    //[SerializeField] GameObject thing;

    [SerializeField] protected KeyCode PickupKey = KeyCode.E; //change for controller input

    [SerializeField] protected GameObjectRuntimeSet playerRuntimeSet;
    protected List<GameObject> players;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider player)
    {
        if (player.gameObject.tag == "Player1")
        {
            isInRange = true;
            uiObject.SetActive(true);

            // Check if the item is not null before enabling the spriteRenderer
            //Debug.LogWarning("in range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        uiObject.SetActive(false);

    }
}
