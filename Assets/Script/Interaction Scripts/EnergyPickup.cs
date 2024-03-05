using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerSet;
    CelestialPlayer celestialPlayer;
    [SerializeField] public int energyQuantity;

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
            Destroy(this.gameObject);
        }
    }
}
