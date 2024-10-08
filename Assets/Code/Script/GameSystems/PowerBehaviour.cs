using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBehaviour : MonoBehaviour
{
    //Power stats
    [SerializeField] public PowerStats ColdSnapStats;
    [SerializeField] public PowerStats LightningStats;
    [SerializeField] public PowerStats BasicAttackStats;
    [SerializeField] public PowerStats MoonTideAttackStats;

    public event System.Action OnPowerStateChange;

    // Start is called before the first frame update
    void Start()
    {
        ColdSnapStats.isOnCooldown = false;
        LightningStats.isOnCooldown = false;
        BasicAttackStats.isOnCooldown = false;
        MoonTideAttackStats.isOnCooldown = false;
        if (OnPowerStateChange != null)
            OnPowerStateChange();
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
        powerStats.isEnabled = true;
        if (OnPowerStateChange != null)
            OnPowerStateChange();
    }

    public void setDisabled(PowerStats powerStats)
    {
        powerStats.isEnabled = false;
        if (OnPowerStateChange != null)
            OnPowerStateChange();
    }


}
