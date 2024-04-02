using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBehaviour : MonoBehaviour
{
    //MonsterStats
    [SerializeField] public PowerStats ColdSnapStats;
    [SerializeField] public PowerStats LightningStats;
    [SerializeField] public PowerStats BasicAttackStats;
    [SerializeField] public PowerStats MoonTideAttackStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }


   public int getMaxDamage(PowerStats powerStats)
    {
        return powerStats.maxDamage;
    }
    public WaitForSeconds getRechargeTimer(PowerStats powerStats)
    {
        return powerStats.rechargeTimer;
    }
    public float getRechargeTimerFloat(PowerStats powerStats)
    {
        return powerStats.rechargeFloatTimer;
    }

    public void setEnabled(PowerStats powerStats)
    {
        Debug.Log("Stat is enabled");
        powerStats.isEnabled = true;
       
    }


}
