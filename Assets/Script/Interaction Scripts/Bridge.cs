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
    private bool previewOn;

    [SerializeField] private float interactDistance;

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
            StartBridgeBuild();
            UpdateUIElement();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CalcDistance();
        if (other.GetComponent<EarthPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            //p1IsInRange = true;
            if (!bridgeIsBuilt)
            {
                ActivateBridgePreview();
            }
        }
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            //p2IsInRange = true;
            if (!bridgeIsBuilt)
            {
                ActivateBridgePreview();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CalcDistance();
        if (other.GetComponent<EarthPlayer>())
        {
            //p1IsInRange = false;
            if (!bridgeIsBuilt)
            {
                DeactivateBridgePreview();
            }
        }
        if (other.GetComponent<CelestialPlayer>())
        {
            //p2IsInRange = false;
            if (!bridgeIsBuilt)
            {
                DeactivateBridgePreview();
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

    private void ActivateBridgePreview()
    {
        previewOn = true;
        bridgeGeometry.SetActive(true);
        pickupTarget.SetActive(true);
        levelOneEvents.OnBridgeEncountered();
    }

    private void DeactivateBridgePreview()
    {
        previewOn = false;
        bridgeGeometry.SetActive(false);
        pickupTarget.SetActive(false);
    }

    private void StartBridgeBuild()
    {
        if(p1IsInRange && earthPlayer.interacting && earthPlayer.inventory.HasEnoughItems("Tree Log", 3))
        {
            bridgeIsBuilt = true;
            pickupTarget.SetActive(false);
            StartCoroutine(BuildBridge());
        }
        else if(p1IsInRange && earthPlayer.interacting)
        {
            NotEnoughLogs();
        }
        else if(p2IsInRange && celestialPlayer.interacting)
        {
            WrongCharacter();
        }
    }

    private IEnumerator BuildBridge()
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
        string warningText = "Not enough logs";
        hudManager.ThrowPlayerWarning(warningText);
        //yield return buildTime;
        //earthPlayer.displayText.text = "";
    }

    private void WrongCharacter()
    {
        string warningText = "Only Sprout can build this";
        hudManager.ThrowPlayerWarning(warningText);
    }
}
