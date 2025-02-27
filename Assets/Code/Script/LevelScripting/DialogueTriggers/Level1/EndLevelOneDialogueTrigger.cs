using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevelOneDialogueTrigger : MonoBehaviour
{
    public DialogueTrigger endMissionDialogue;
    [SerializeField] private HUDModel hudModel;
    //[SerializeField] public TextMeshProUGUI displayText;
    bool playerOnePresent;
    bool playerTwoPresent;
    bool hasRunDialogue;

    private void Update()
    {
        if (!hasRunDialogue)
        {
            RunMissionEnd();
            ShowText();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if earthPlayer enterted area
        if (other.CompareTag("Player1") && other is CapsuleCollider)
        {
            playerOnePresent = true;
        }
        else if (other.CompareTag("Player2") && other is CapsuleCollider)
        {
            playerTwoPresent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") && other is CapsuleCollider)
        {
            playerOnePresent = false;
        }
        else if (other.CompareTag("Player2") && other is CapsuleCollider)
        {
            playerTwoPresent = false;
        }
    }

    //If only one of the players is standing in the circle at the end, tell them what they're doing wrong
    private void ShowText()
    {
        if((playerOnePresent && !playerTwoPresent) || (!playerOnePresent && playerTwoPresent))
        {
            hudModel.GetManager().ThrowPlayerWarning("Both players must be present to leave", "Les deux joueurs doivent �tre pr�sents pour partir");
            //displayText.text = "Both players must be present to leave";
        }
        /*
        else if(!playerOnePresent && !playerTwoPresent)
        {
            //hudModel.GetManager().TurnOffPopUpText();
            //displayText.text = "";
        }
        */
        
    }

    //Trigger the end of mission dialogue
    private void RunMissionEnd()
    {
        if(playerOnePresent && playerTwoPresent)
        {
            hasRunDialogue = true;
            endMissionDialogue.TriggerDialogue();
        }
    }
}
