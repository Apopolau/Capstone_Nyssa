using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ladder : Interactable
{
    [SerializeField] LevelTwoEvents levelTwoEvents;
    [SerializeField] GameObject pickupTarget;
    [SerializeField] GameObject ladderGeometry;
    Material material;
    [SerializeField] Material transparentMaterial;
    WaitForSeconds buildTime = new WaitForSeconds(4.542f);

    private bool ladderIsBuilt = false;
    private bool previewOn;
    private bool canBeClimbed = false;

    [SerializeField] DialogueTrigger earthClimbUp;
    [SerializeField] DialogueTrigger earthClimbDown;
    [SerializeField] DialogueTrigger celestialClimbUp;
    [SerializeField] DialogueTrigger celestialClimbDown;

    [SerializeField] private float interactDistance;

    [SerializeField] GameObject popupText;
    [SerializeField] GameObject logImage;

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
        }
        if (other.GetComponent<CelestialPlayer>())
        {
            //p2IsInRange = false;
            if (!ladderIsBuilt)
            {
                DeactivateLadderPreview();
            }
        }
    }

    private void CalcDistance()
    {
        if(Mathf.Abs((earthPlayer.GetGeo().transform.position - this.transform.position).magnitude) < interactDistance)
        {
            p1IsInRange = true;
        }
        else
        {
            p1IsInRange = false;
        }

        if (Mathf.Abs((celestialPlayer.GetGeo().transform.position - this.transform.position).magnitude) < interactDistance)
        {
            p2IsInRange = true;
        }
        else
        {
            p2IsInRange = false;
        }
    }

    private void ActivateLadderPreview()
    {
        previewOn = true;
        ladderGeometry.SetActive(true);
        pickupTarget.SetActive(true);
        //levelTwoEvents.OnLadderEncountered();
    }

    private void DeactivateLadderPreview()
    {
        previewOn = false;
        ladderGeometry.SetActive(false);
        pickupTarget.SetActive(false);
    }

    private void StartLadderBuild()
    {
        if(p1IsInRange && earthPlayer.interacting && earthPlayer.inventory.HasEnoughItems("Tree Log", 3))
        {
            ladderIsBuilt = true;
            pickupTarget.SetActive(false);
            StartCoroutine(BuildLadder());
        }
        else if(p1IsInRange && earthPlayer.interacting)
        {
            StartCoroutine(NotEnoughLogs());
        }
        else if(p2IsInRange && celestialPlayer.interacting)
        {
            StartCoroutine(WrongCharacter());
        }
    }

    private IEnumerator BuildLadder()
    {
        //Get the player turning towards the bridge as they start building
        earthPlayer.SetTurnTarget(this.transform.position);
        earthPlayer.ToggleTurning();
        //Set animations
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, true);

        //Initiate state change
        earthPlayer.SetSuspensionTime(buildTime);
        earthPlayer.ToggleInteractingState();
        //earthPlayer.CallSuspendActions(buildTime);
        yield return buildTime;
        earthPlayer.ToggleInteractingState();
        earthPlayer.ToggleTurning();

        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, false);
        FinishLadderBuild();
    }

    private void FinishLadderBuild()
    {
        earthPlayer.inventory.RemoveItemByName("Tree Log", 3);
        ladderGeometry.GetComponentInChildren<MeshRenderer>().material = material;
        //levelTwoEvents.OnBridgeBuilt();
        ladderIsBuilt = true;
        popupText.GetComponent<TextMeshPro>().text = "Climb";
        logImage.SetActive(false);
        //Destroy(this.gameObject.GetComponent<BoxCollider>());
    }

    private IEnumerator NotEnoughLogs()
    {
        earthPlayer.displayText.text = "Not enough logs";
        yield return buildTime;
        earthPlayer.displayText.text = "";
    }

    private IEnumerator WrongCharacter()
    {
        earthPlayer.displayText.text = "Only Sprout can build this";
        yield return buildTime;
        earthPlayer.displayText.text = "";
    }

    private void TriggerLadderClimb()
    {
        if (ladderIsBuilt && (p1IsInRange && earthPlayer.interacting))
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
        else if(ladderIsBuilt && (p2IsInRange && celestialPlayer.interacting))
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
