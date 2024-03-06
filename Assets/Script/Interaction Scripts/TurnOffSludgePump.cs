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
    private WaitForSeconds turnTime = new WaitForSeconds(4.958f);
    private bool earthDialogueHasPlayed = false;
    private bool celestialDialogueHasPlayed = false;


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
            StartCoroutine(SludgePumpTurnsOff());
             //trigger dialouge after sludge is turned off
        }
    }

    private IEnumerator SludgePumpTurnsOff()
    {
        earthPlayer.GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfTurningHash, true);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfWalkingHash, false);
        earthPlayer.CallSuspendActions(turnTime);
        yield return turnTime;
        Debug.Log("Turned off the sludge pump");
        levelOneEvents.OnPumpShutOff();
        uiObject.SetActive(false);
        sludgeOffDialouge.TriggerDialogue();
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfTurningHash, false);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfWalkingHash, true);
    }


    // trigger dialouge when pump is encountred
     private void OnTriggerEnter(Collider other)
    {
        // Check if earthPlayer enterted area
        if (other.CompareTag("Player1") && !earthDialogueHasPlayed)
        {   
            sludgeDialougeEarth.TriggerDialogue();
            earthDialogueHasPlayed = true;
            celestialDialogueHasPlayed = true;
        }
        else if (other.CompareTag("Player2") && (!earthDialogueHasPlayed && !celestialDialogueHasPlayed))
        {
           sludgeDialougeCelest.TriggerDialogue();
           celestialDialogueHasPlayed = true;
        }
    }
}
