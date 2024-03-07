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
        if (other.GetComponent<CelestialPlayer>())
        {
            celestialPlayer.energy.current += energyQuantity;
            // Debug log the energy quantity
            Debug.Log("Energy quantity increased by: " + energyQuantity);
            Destroy(this.gameObject);
            
            IncreaseEnergy();
           
        }
    }

    private void IncreaseEnergy()
    {
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
                Debug.LogWarning("Fill Image component not found under energyBar GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Energy bar GameObject not found.");
        }
    }
}
