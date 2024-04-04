using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffTerminal : Interactable
{
    [SerializeField] LevelTwoEvents levelTwoEvents;
    [SerializeField] DialogueTrigger waterCleanedDialogue;
    Color scorchColor = new Color(50, 50, 50);

    bool hasBeenShutOff;

    private void TerminalShutOff() 
    {
        if (!hasBeenShutOff)
        {
            levelTwoEvents.OnFacilityShutOff();
            waterCleanedDialogue.TriggerDialogue();
            hasBeenShutOff = true;
            GetComponent<Material>().color = scorchColor;
        }
        
    }
}
