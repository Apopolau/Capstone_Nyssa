using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdSnapBehaviour : MonoBehaviour
{
    //MonsterStats
    [SerializeField] public PowerStats ColdSnapStats;
    [SerializeField] public PowerStats LightningStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }


   public int getMaxDamage(PowerStats powerStats)
    {
        return powerStats.maxDamage;
    }
    public float getRechargeTimer(PowerStats powerStats)
    {
        return powerStats.rechargeTimer;
    }


}
