using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    

    void Start()
    {
        //TriggerDialogue();
    }

     /* void StartDialogue()
    {
        // Ensure that the DialogueManager instance is available
        if (DialogueManager.Instance != null)
        {
            // Call your Dialogue Manager and pass the dialogue data to initiate the dialogue.
            DialogueManager.Instance.StartDialogue(dialogue);
            Debug.LogError("DialogueManager started");
        }
        else
        {
            Debug.LogError("DialogueManager instance not found. Make sure the DialogueManager script is attached to an object in your scene.");
        }
    } */


     public void TriggerDialogue()
    {


        DialogueManager.Instance.StartDialogue(dialogue);
        Debug.Log("DialogueManager started");
       // uiController.DeactivateAllUI();
    }

   
 
   /* private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player1")
        {
           TriggerDialogue();
        }
    } */
}
