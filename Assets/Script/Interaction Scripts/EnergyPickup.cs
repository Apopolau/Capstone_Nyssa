using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPickup : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerSet;
    CelestialPlayer celestialPlayer;
    [SerializeField] public int energyQuantity;
    // [SerializeField] private Image energyBarFill; // Reference energy bar fill
    bool isBeingAbsorbed = false;

    WaitForSeconds absorbDelay = new WaitForSeconds(0.1f);

    private void Awake()
    {
        foreach(GameObject player in playerSet.Items)
        {
            if (player.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = player.GetComponent<CelestialPlayer>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CelestialPlayer>() && other.GetType() == typeof(CapsuleCollider))
        {
            //celestialPlayer.energy.current += energyQuantity;
            IncreaseEnergy();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyNeighbouringObject(Collider other)
    {
        yield return absorbDelay;
        //quantity += other.GetComponent<PickupObject>().quantity;
        energyQuantity++;
        Destroy(other.gameObject);
    }

    private void IncreaseEnergy()
    {
        celestialPlayer.IncreaseEnergy(energyQuantity);
        /*
        // Find the energy bar fill Image component dynamically
        GameObject energyBar = GameObject.Find("EnergyBar"); // Assuming "energyBar" is the name of the GameObject holding the fill Image
        if (energyBar != null)
        {
            Image fillImage = energyBar.transform.Find("Fill").GetComponent<Image>(); // Assuming "fill" is the name of the Image GameObject representing the fill
            if (fillImage != null)
            {
                // Calculate the new fill amount
                float fillAmount = fillImage.fillAmount + (float)energyQuantity / 100f; // Assuming energy bar's max value is 100
                fillImage.fillAmount = Mathf.Clamp01(fillAmount); // Clamp fill amount between 0 and 1
            }
            else
            {
                
            }
        }
        else
        {
            
        }
        */
    }
}
