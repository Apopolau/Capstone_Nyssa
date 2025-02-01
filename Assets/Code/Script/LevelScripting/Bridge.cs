using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Interactable
{
    [SerializeField] LevelOneEvents levelOneEvents;
    [SerializeField] GameObject pickupTarget;
    [SerializeField] GameObject bridgeGeometry;
    [SerializeField] GameObject bridgeCollider;
    Material material;
    [SerializeField] Material transparentMaterial;
    WaitForSeconds buildTime = new WaitForSeconds(4.542f);

    private bool bridgeIsBuilt = false;
    //private bool previewOn;

    

    private void Awake()
    {
        isEarthInteractable = true;
        material = bridgeGeometry.GetComponentInChildren<MeshRenderer>().material;
        bridgeCollider = bridgeGeometry.GetComponentInChildren<MeshCollider>().gameObject;
        bridgeCollider.SetActive(false);
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
        bridgeGeometry.GetComponentInChildren<MeshRenderer>().material = transparentMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bridgeIsBuilt)
        {
            CalcDistance();
            if(p1IsInRange || p2IsInRange)
            {
                ActivateBridgePreview();
            }
            else
            {
                DeactivateBridgePreview();
            }
            StartBridgeBuild();
            UpdateUIElement();
        }
    }

    private void ActivateBridgePreview()
    {
        //previewOn = true;
        bridgeGeometry.SetActive(true);
        //pickupTarget.SetActive(true);
        levelOneEvents.OnBridgeEncountered();
    }

    private void DeactivateBridgePreview()
    {
        //previewOn = false;
        bridgeGeometry.SetActive(false);
        //pickupTarget.SetActive(false);
    }

    private void StartBridgeBuild()
    {
        if(p1IsInRange && earthPlayer.GetIsInteracting() && earthPlayer.inventory.HasEnoughItems("Tree Log", 3))
        {
            bridgeIsBuilt = true;
            pickupTarget.SetActive(false);
            StartCoroutine(BuildBridge());
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

    private IEnumerator BuildBridge()
    {
        //Get the player turning towards the bridge as they start building
        earthPlayer.SetTurnTarget(this.gameObject);
        earthPlayer.ToggleTurning(true);
        //Set animations
        earthPlayer.GetComponent<EarthPlayerAnimator>().SetAnimationFlag("build", true);

        //Initiate state change
        earthPlayer.ToggleInteractingState();
        yield return buildTime;
        earthPlayer.ToggleInteractingState();
        earthPlayer.ToggleTurning(false);

        FinishBridgeBuild();
    }

    private void FinishBridgeBuild()
    {
        earthPlayer.inventory.RemoveItemByName("Tree Log", 3);
        bridgeCollider.SetActive(true);
        bridgeGeometry.GetComponentInChildren<MeshRenderer>().material = material;
        levelOneEvents.OnBridgeBuilt();
        Destroy(this.gameObject.GetComponent<BoxCollider>());
    }

    private void NotEnoughLogs()
    {
        string enWarningText = "Not enough logs";
        string frWarningText = "Pas assez du bûche";
        hudManager.ThrowPlayerWarning(enWarningText, frWarningText);
        //yield return buildTime;
        //earthPlayer.displayText.text = "";
    }

    private void WrongCharacter()
    {
        string enWarningText = "Only Sprout can build this";
        string frWarningText = "Seul Sprout peut construire ceci";
        hudManager.ThrowPlayerWarning(enWarningText, frWarningText);
    }
}
