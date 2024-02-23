using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Interactable
{
    [SerializeField] GameObject pickupTarget;
    [SerializeField] GameObject bridgeGeometry;
    [SerializeField] GameObjectRuntimeSet playerSet;
    Material material;
    [SerializeField] Material transparentMaterial;
    EarthPlayer earthPlayer;
    WaitForSeconds buildTime = new WaitForSeconds(4.542f);

    private bool bridgeIsBuilt = false;
    private bool previewOn;

    private void Awake()
    {
        material = bridgeGeometry.GetComponentInChildren<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject p in playerSet.Items)
        {
            if (p.GetComponent<EarthPlayer>())
            {
                earthPlayer = p.GetComponent<EarthPlayer>();
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EarthPlayer>())
        {
            isInRange = true;
            if (!bridgeIsBuilt)
            {
                ActivateBridgePreview();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EarthPlayer>())
        {
            isInRange = false;
            if (!bridgeIsBuilt)
            {
                DeactivateBridgePreview();
            }
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
        if(isInRange && earthPlayer.interacting && earthPlayer.inventory.HasEnoughItems("Tree Log", 3))
        {
            bridgeIsBuilt = true;
            pickupTarget.SetActive(false);
            StartCoroutine(BuildBridge());
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
        bridgeGeometry.GetComponentInChildren<MeshRenderer>().material = material;
        Destroy(this.gameObject.GetComponent<BoxCollider>());
    }
}
