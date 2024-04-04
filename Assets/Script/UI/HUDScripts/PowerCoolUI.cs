using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerCoolUI : MonoBehaviour
{
    [SerializeField] public CelestialPlayer celestialPlayer; // Reference to your CelestialPlayer script
    [SerializeField] public Image radialFillImage; // Reference to the UI Image component

    private bool isCoolingDown = false;
    private float cooldownDuration = 10f; // Adjust this value to match your cooldown time

    private void Start()
    {
//        radialFillImage.fillAmount = 1f; // Set initial fill amount to 1
    }

    private void Update()
    {
        if (isCoolingDown)
        {
            // Decrease fill amount gradually over cooldown duration
            radialFillImage.fillAmount -= Time.deltaTime / cooldownDuration;

            // Check if cooldown is over
            if (radialFillImage.fillAmount <= 0f)
            {
                radialFillImage.fillAmount = 0f; // Ensure fill amount is exactly 0
                isCoolingDown = false;
                celestialPlayer.canColdSnap = true; // Allow ColdSnap power to be used again
            }
        }
    }

    // Method to start cooldown
    public void StartCooldown()
    {
        isCoolingDown = true;
    }
}
