using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ladder : Interactable
{
    [SerializeField] LevelTwoEvents levelTwoEvents;
    [SerializeField] UserSettingsManager userSettingsManager;
    [SerializeField] GameObject pickupTarget;
    [SerializeField] GameObject ladderGeometry;
    Material material;
    [SerializeField] Material transparentMaterial;
    WaitForSeconds buildTime = new WaitForSeconds(4.542f);

    private bool ladderIsBuilt = false;
    //private bool previewOn;
    //private bool canBeClimbed = false;

    [SerializeField] DialogueTrigger earthClimbUp;
    [SerializeField] DialogueTrigger earthClimbDown;
    [SerializeField] DialogueTrigger celestialClimbUp;
    [SerializeField] DialogueTrigger celestialClimbDown;

    [SerializeField] GameObject logReqs;

    private void Awake()
    {
        isEarthInteractable = true;
        material = ladderGeometry.GetComponentInChildren<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
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
        ladderGeometry.GetComponentInChildren<MeshRenderer>().material = transparentMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ladderIsBuilt)
        {
            StartLadderBuild();
            
        }
        UpdateUIElement();
    }

    private void OnTriggerStay(Collider other)
    {
        CalcDistance();
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            //p1IsInRange = true;
            if (!ladderIsBuilt)
            {
                ActivateLadderPreview();
            }
            if (ladderIsBuilt)
            {
                TriggerLadderClimb();
                TurnOnPopup();
            }
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            //p2IsInRange = true;
            if (!ladderIsBuilt)
            {
                ActivateLadderPreview();
            }
            if (ladderIsBuilt)
            {
                TriggerLadderClimb();
                TurnOnPopup();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CalcDistance();
        if (other.GetComponent<EarthPlayer>())
        {
            //p1IsInRange = false;
            if (!ladderIsBuilt)
            {
                DeactivateLadderPreview();
            }
            if (ladderIsBuilt)
            {
                TurnOffPopup();
            }
        }
        if (other.GetComponent<CelestialPlayer>())
        {
            //p2IsInRange = false;
            if (!ladderIsBuilt)
            {
                DeactivateLadderPreview();
            }
            if (ladderIsBuilt)
            {
                TurnOffPopup();
            }
        }
    }

    private void ActivateLadderPreview()
    {
        //previewOn = true;
        ladderGeometry.SetActive(true);
        TurnOnPopup();
        //levelTwoEvents.OnLadderEncountered();
    }

    private void DeactivateLadderPreview()
    {
        //previewOn = false;
        ladderGeometry.SetActive(false);
        TurnOffPopup();
    }

    private void TurnOnPopup()
    {
        pickupTarget.SetActive(true);
    }

    private void TurnOffPopup()
    {
        pickupTarget.SetActive(false);
    }

    private void StartLadderBuild()
    {
        if(p1IsInRange && earthPlayer.GetIsInteracting() && earthPlayer.inventory.HasEnoughItems("Tree Log", 3))
        {
            pickupTarget.SetActive(false);
            StartCoroutine(BuildLadder());
        }
        else if(p1IsInRange && earthPlayer.GetIsInteracting())
        {
            NotEnoughLogs();
        }
        else if(p2IsInRange && celestialPlayer.GetIsInteracting())
        {
            WrongCharacter();
        }
    }

    private IEnumerator BuildLadder()
    {
        //Get the player turning towards the bridge as they start building
        earthPlayer.SetTurnTarget(this.transform.position);
        earthPlayer.ToggleTurning(true);
        //Set animations
        //earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, true);
        earthPlayer.GetComponent<EarthPlayerAnimator>().SetAnimationFlag("build", true);

        //Initiate state change
        earthPlayer.SetSuspensionTime(buildTime);
        earthPlayer.ToggleInteractingState();
        //earthPlayer.CallSuspendActions(buildTime);
        yield return buildTime;
        earthPlayer.ToggleInteractingState();
        earthPlayer.ToggleTurning(false);

        //earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, false);
        earthPlayer.GetComponent<EarthPlayerAnimator>().SetAnimationFlag("build", false);
        FinishLadderBuild();
    }

    private void FinishLadderBuild()
    {
        earthPlayer.inventory.RemoveItemByName("Tree Log", 3);
        ladderGeometry.GetComponentInChildren<MeshRenderer>().material = material;
        //levelTwoEvents.OnBridgeBuilt();
        ladderIsBuilt = true;
        if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.ENGLISH)
        {
            interactPrompt.ChangeText("Climb");
        }
        else if(userSettingsManager.chosenLanguage == UserSettingsManager.GameLanguage.FRENCH)
        {
            interactPrompt.ChangeText("Grimper");
        }
        //popupText.GetComponent<TextMeshProUGUI>().text = "Climb";
        isCelestialInteractable = true;
        logReqs.SetActive(false);
        //Destroy(this.gameObject.GetComponent<BoxCollider>());
    }

    private void NotEnoughLogs()
    {
        string enWarningText = "Not enough logs";
        string frWarningText = "Pas assez de bûche";
        hudManager.ThrowPlayerWarning(enWarningText, frWarningText);
    }

    private void WrongCharacter()
    {
        string enWarningText = "Only Sprout can build this";
        string frWarningText = "Seul Sprout peut construire ceci";
        hudManager.ThrowPlayerWarning(enWarningText, frWarningText);
    }

    private void TriggerLadderClimb()
    {
        if (ladderIsBuilt && (p1IsInRange && earthPlayer.GetIsInteracting()))
        {
            if(earthPlayer.transform.position.y > 5)
            {
                earthClimbDown.TriggerDialogue();
            }
            else
            {
                earthClimbUp.TriggerDialogue();
            }
        }
        else if(ladderIsBuilt && (p2IsInRange && celestialPlayer.GetIsInteracting()))
        {
            if (celestialPlayer.transform.position.y > 5)
            {
                celestialClimbDown.TriggerDialogue();
            }
            else
            {
                celestialClimbUp.TriggerDialogue();
            }
        }
    }
}
