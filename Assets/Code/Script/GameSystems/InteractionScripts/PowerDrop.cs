using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDrop : Interactable
{
    public CelestialPlayer.Power powerDrop;

    //public CelestialPlayer.coldSnapFill coldSnapFill;
    //[SerializeField] public Image coldSnapFill;
    public PowerBehaviour powerBehaviour;
    [SerializeField] private LevelProgress levelProgress;
    [SerializeField] private HUDModel hudModel;
    /// <summary>
    /// start a dialogue
    /// Tell Celest ow to use it
    /// enable the particular power up 
    /// </summary>
    // Start is called before the first frame update
    private void Awake()
    {
        hudManager = hudModel.GetManager();
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

    private void Start()
    {
        powerBehaviour = GetComponent<PowerBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {
        CalcDistance();
        UpdateUIElement();
        PowerPickup();
    }

    /// <summary>
    /// APOLINE use this function for handling what happens when the player tries to pick up the power
    /// </summary>
    public void PowerPickup()
    {

        if (p2IsInRange && celestialPlayer.GetIsInteracting())
        {
            PowerStats currPowerStats = getPowerDropStatRef(powerDrop);
            powerBehaviour.setEnabled(currPowerStats);


            //////////////////////////////////Will need to create a new UI for this

            // Destroy(this.GetComponentInParent<Transform>().gameObject);
            Destroy(this.gameObject);

        }
    }

    private PowerStats getPowerDropStatRef(CelestialPlayer.Power powerDrop)
    {
        if (powerDrop == CelestialPlayer.Power.COLDSNAP)
        {
            levelProgress.SetPowers(true);
            /*
            celestialPlayer.coldSnapFill.enabled = false;
            celestialPlayer.CTRLColdSnapFill.enabled = false;
            StartCoroutine(celestialPlayer.CoolDownImageFill(celestialPlayer.coldSnapFill));
            celestialPlayer.coldSnapFill.enabled = true;
            celestialPlayer.CTRLColdSnapFill.enabled = true;
            */
            hudManager.TurnOffPowerOverlay("CastCold");
            return powerBehaviour.ColdSnapStats;
        }
        if (powerDrop == CelestialPlayer.Power.MOONTIDE)
        {
            levelProgress.SetPowers(true);
            /*
            celestialPlayer.moonTideFill.enabled = false;
            celestialPlayer.CTRLMoonTideFill.enabled = false;
            StartCoroutine(celestialPlayer.CoolDownImageFill(celestialPlayer.moonTideFill));
            celestialPlayer.moonTideFill.enabled = true;
            celestialPlayer.moonTideFill.enabled = true;
            celestialPlayer.CTRLMoonTideFill.enabled = true;
            */
            hudManager.TurnOffPowerOverlay("CastMoontide");
            return powerBehaviour.MoonTideAttackStats;
        }
        return null;
    }

}
