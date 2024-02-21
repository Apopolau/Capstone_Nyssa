using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
   // Icon for the left side of the dialogue box
    public Sprite iconLeft;

    // Icon for the right side of the dialogue box
    public Sprite iconRight;

    // Whether the character should appear on the left side of the dialogue box
    public bool isLeft;
}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
    [TextArea(3, 10)]
    public string lineFR; // Text for French language
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}


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
    }

   
 
   /* private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player1")
        {
           TriggerDialogue();
        }
    } */
}
