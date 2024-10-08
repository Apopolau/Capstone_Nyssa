using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private HUDModel hudModel;
    [SerializeField] private DialogueManager dialogueManager;
    public Dialogue dialogue;

    private void Awake()
    {
        hudModel.AddDialogueTrigger(this);
    }

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }

    public void SetDialogueManager(DialogueManager dialogueManager)
    {
        this.dialogueManager = dialogueManager;
    }

    public DialogueManager GetDialogueManager()
    {
        return dialogueManager;
    }
}
