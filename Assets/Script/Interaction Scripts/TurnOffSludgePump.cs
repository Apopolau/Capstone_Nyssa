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
    private bool sludgePumpIsOff = false;

    private void Awake()
    {
        isEarthInteractable = true;
    }

    private void Start()
    {
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
        
    }

    private void Update()
    {
        if (!sludgePumpIsOff)
        {
            TurnOff();
            UpdateUIElement();
        }
        
    }

    public void TurnOff()
    {
        if (p1IsInRange && earthPlayer.interacting)
        {
            StartCoroutine(SludgePumpTurnsOff());
             //trigger dialouge after sludge is turned off
        }
    }

    private IEnumerator SludgePumpTurnsOff()
    {
        /*
        Vector3 viewDir = earthPlayer.transform.position - new Vector3(transform.position.x, earthPlayer.transform.position.y, transform.position.z);
        if (viewDir != Vector3.zero)
        {
            earthPlayer.GetComponent<playerMovement>().orientation.forward = viewDir.normalized;
        }
        */
        //earthPlayer.GetComponent<playerMovement>().playerObj.transform.LookAt()
        earthPlayer.GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfTurningHash, true);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfWalkingHash, false);
        earthPlayer.CallSuspendActions(turnTime);
        yield return turnTime;
        sludgePumpIsOff = true;
        Debug.Log("Turned off the sludge pump");
        levelOneEvents.OnPumpShutOff();
        uiObject.SetActive(false);
        sludgeOffDialouge.TriggerDialogue();
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfTurningHash, false);
        //earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfWalkingHash, true);
    }


    // trigger dialouge when pump is encountred
     private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = true;
        }
        else if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = true;
        }

        // Check if earthPlayer enterted area
        if (p1IsInRange && !earthDialogueHasPlayed)
        {   
            sludgeDialougeEarth.TriggerDialogue();
            earthDialogueHasPlayed = true;
            celestialDialogueHasPlayed = true;
        }
        else if (p2IsInRange && (!earthDialogueHasPlayed && !celestialDialogueHasPlayed) && other is CapsuleCollider)
        {
           sludgeDialougeCelest.TriggerDialogue();
           celestialDialogueHasPlayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p1IsInRange = false;
        }
        else if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            p2IsInRange = false;
        }
    }
}
