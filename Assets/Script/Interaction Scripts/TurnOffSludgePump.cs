using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSludgePump : Interactable
{
    [SerializeField] MeshRenderer m_Renderer;
    //[SerializeField] Item item;
    //[SerializeField] Inventory inventory;
    [SerializeField] LevelOneEvents levelOneEvents;
    public DialogueTrigger sludgeOffDialouge;
    public DialogueTrigger sludgeDialougeEarth;
    public DialogueTrigger sludgeDialougeCelest;


    //public GameObject boxRange;

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
            Debug.Log("Turned off the sludge pump");
            levelOneEvents.OnPumpShutOff();
            uiObject.SetActive(false);
            sludgeOffDialouge.TriggerDialogue(); //trigger dialouge after sludge is turned off
        }
    }


    // trigger dialouge when pump is encountred
     private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if (other.CompareTag("Player1"))
        {   
            sludgeDialougeEarth.TriggerDialogue();
            // Destroy the GameObject collider
           // Destroy(gameObject);
        }
        else if (other.CompareTag("Player2"))
        {
           sludgeDialougeCelest.TriggerDialogue();
            // Destroy the GameObject collider
            //Destroy(gameObject);
        }
    }
}
