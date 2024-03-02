using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Interactable
{
    [SerializeField] GameObject pickupTarget;
    [SerializeField] GameObject bridgeGeometry;
    [SerializeField] GameObject bridgeCollider;
    Material material;
    [SerializeField] Material transparentMaterial;
    WaitForSeconds buildTime = new WaitForSeconds(4.542f);

    private bool bridgeIsBuilt = false;
    private bool previewOn;

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
        if (other.GetComponent<EarthPlayer>())
        {
            p1IsInRange = true;
            if (!bridgeIsBuilt)
            {
                ActivateBridgePreview();
            }
        }
        if (other.GetComponent<CelestialPlayer>())
        {
            p2IsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EarthPlayer>())
        {
            p1IsInRange = false;
            if (!bridgeIsBuilt)
            {
                DeactivateBridgePreview();
            }
        }
        if (other.GetComponent<CelestialPlayer>())
        {
            p2IsInRange = false;
        }
    }

    private void ActivateBridgePreview()
    {
        previewOn = true;
        bridgeGeometry.SetActive(true);
        pickupTarget.SetActive(true);
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
            StartCoroutine(NotEnoughLogs());
        }
    }

    private IEnumerator BuildBridge()
    {
        earthPlayer.GetComponent<playerMovement>().playerObj.transform.LookAt(this.transform);
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, true);
        earthPlayer.CallSuspendActions(buildTime);
        yield return buildTime;
        earthPlayer.earthAnimator.animator.SetBool(earthPlayer.earthAnimator.IfBuildingHash, false);
        FinishBridgeBuild();
    }

    private void FinishBridgeBuild()
    {
        earthPlayer.inventory.RemoveItemByName("Tree Log", 3);
        bridgeCollider.SetActive(true);
        bridgeGeometry.GetComponentInChildren<MeshRenderer>().material = material;
        Destroy(this.gameObject.GetComponent<BoxCollider>());
    }

    private IEnumerator NotEnoughLogs()
    {
        earthPlayer.displayText.text = "Not enough logs";
        yield return buildTime;
        earthPlayer.displayText.text = "";
    }
}
