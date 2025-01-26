using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;
using UnityEngine.VFX;
public class CelestialPlayer : Player
{
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    private bool isRaining = false;
    private bool triggeredRain = false;
    [SerializeField] public GameObject RainParticleSystem;

    [Header("Attack")]
    CelestialPlayerBasicAttackTrigger staff;
    private bool isAttacking = false;
    private bool canBasicAttack = true;
    private bool canColdSnap = true;
    private bool canLightningStrike = true;
    private bool canMoonTide = true;
    //Reactivate these if we implement them
    //private bool canSetFogTrap = true;
    //private bool canSetFrostTrap = true;
    //private bool canSunBeam = true;


    private WaitForSeconds coldOrbDuration;
    private WaitForSeconds waveDuration;
    private WaitForSeconds lightningDuration;

    private WaitForSeconds energyRegenTimer = new WaitForSeconds(5);

    //private bool isTargeted = false;

    public enum Power
    {
        NONE,
        BASIC,
        COLDSNAP,
        FOGTRAP,
        FROSTTRAP,
        LIGHTNINGSTRIKE,
        MOONTIDE,
        SUNBEAM
    };

    public Power Powers;
    public Power powerInUse = Power.NONE;

    [Header("Energy variables")]
    private Stat energy;
    [SerializeField] private int maxEnergyVal;
    [SerializeField] private int startingEnergyVal;
    public event System.Action<int, int> OnEnergyChanged;
    public event System.Action OnPowerStateChange;
  
    PowerBehaviour powerBehaviour;
    public CelestialPlayerControls celestialControls;

    [Header("Storage for power objects")]
    GameObject coldOrb;
    GameObject moonTide;
    GameObject lightning;

    [SerializeField] private Vector3 moontideOffset;
    [SerializeField] private float lightningOffset;
    //[SerializeField] private float lightningRange;
    [SerializeField] private float lightningAngle;

    [SerializeField] private CelesteSoundLibrary c_soundLibrary;

    [SerializeField] public GameObject treeSeedPrefab;
    private PlayerInput playerInput;
 
    //Interaction with the player
    [SerializeField] private GameObjectRuntimeSet enemyList;
    public bool enemySeen = false;
    public bool enemyHit = false;
    public Vector3 enemyLocation;



    private void Awake()
    {
        OrigPos = this.gameObject.transform.position;
        animator = GetComponent<CelestialPlayerAnimator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        celestialControls = GetComponent<CelestialPlayerControls>();
        health = new Stat(100, 100, false);
        energy = new Stat(maxEnergyVal, startingEnergyVal, true);
        c_soundLibrary = base.soundLibrary as CelesteSoundLibrary;

        //Change these to modify how long her spells appear on screen and can hit things
        coldOrbDuration = new WaitForSeconds(1.5f);
        waveDuration = new WaitForSeconds(1f);
        lightningDuration = new WaitForSeconds(0.5f);
    }

    void Start()
    {
        powerBehaviour = GetComponent<PowerBehaviour>();
        validTargets = new List<GameObject>();
        
        staff = GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();
        staff.SetPlayer(this);
        StartCoroutine(RegenEnergy());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isTargeted)
        {
            ShootTowardsTarget(coldOrb);
        }
        */
    }

    //POWER BUTTONS (HANDLES BUTTON PRESSES)
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    /// Celestial Player Control >> Power buttons are selected >> Heads int FSM CAN_____powersname____
    /// ATTACKS
    public void OnSnowFlakeSelected()
    {
        //COLDSNAP POWER
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        //buttonColdSnap = true;
        if (CheckIfCastable(powerBehaviour.ColdSnapStats, false))
        {
            DrainEnergy(powerBehaviour.ColdSnapStats.energyDrain);
            powerInUse = Power.COLDSNAP;
        }
            
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    public void OnBasicAttackSelected()
    {
        //BASICATTACK POWER
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        //buttonBasicAttack = true;
        if (CheckIfCastable(powerBehaviour.BasicAttackStats, false))
        {
            
            powerInUse = Power.BASIC;
        }
            
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    public void OnLightningStrikeSelected()
    {
        //LIGHTNINGSTRIKE POWER 
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        //buttonLightningStrike = true;
        if (CheckIfCastable(powerBehaviour.LightningStats, false))
        {
            DrainEnergy(powerBehaviour.LightningStats.energyDrain);
            powerInUse = Power.LIGHTNINGSTRIKE;
        }
            
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    public void OnMoonTideSelected()
    {
        //MOONTIDE POWER
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        //buttonMoonTide = true;
        if (CheckIfCastable(powerBehaviour.MoonTideAttackStats, false))
        {
            DrainEnergy(powerBehaviour.MoonTideAttackStats.energyDrain);
            powerInUse = Power.MOONTIDE;
        }
            
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    //if player selects raindrop
    public void OnRainDropSelected()
    {
        if (!isRaining)
        {
            // RainParticleSystem.SetActive(true);
            if(!NotEnoughEnergy(1, false))
            {
                weatherState.SetRainyState(true);
                isRaining = true;
                triggeredRain = true;
            }
            //We could replace this with some other effect to show this is turned on?
            //rainFill.enabled = true;
            //CTRLRainFill.enabled = true;
        }
        else if (isRaining)
        {
            // yield return new WaitForSeconds(10f);

            weatherState.SetRainyState(false);
            RainParticleSystem.SetActive(false);

            isRaining = false;
            //rainFill.enabled = false;
            //CTRLRainFill.enabled = false;

        }
        //buttonRain = true;

    }

    /// OTHER BUTTONS
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("Interacting");
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            //Debug.Log("Not interacting");
            interacting = false;
        }
    }

    //When the player presses the dodge button
    public void OnDodgeSelected(InputAction.CallbackContext context)
    {
        //StartCoroutine(AnimateDodge());
        SetDodge();
    }


    //POWER ANIMATION
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
    /// Celestial Player Control >> Powers are animated>> Heads into Reseting the Power 
    /// 

    //DEPRECATED (check "SetBasicAttackAnimation")
    public IEnumerator animateBasicAttack()
    {
        //Set flags
        animator.SetAnimationFlag("attack", true);

        StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("attack")));
        c_soundLibrary.PlayAttackClips();

        //Wait for the animation to finish
        yield return animator.GetAnimationWaitTime("attack");

        //Reset flags
        animator.SetAnimationFlag("attack", false);
        isAttacking = false;
    }

    public void SetBasicAttackAnimation()
    {
        c_soundLibrary.PlayAttackClips();
        animator.SetAnimationFlag("attack", true);
        SuspendActions(true);
    }



    //COLDSNAP POWER
    public IEnumerator animateColdSnap()
    {
        //Create the actual cold snap prefab
        InitializeColdSnap();

        //Attacking animation of player
        SetCastAnimation();

        //If enemy is around make the cold orb target said enemy >> update
        /*
        if (enemyTarget != null)
        {
            isTargeted = true;
            ColdSnapAttack();
        }
        */
        c_soundLibrary.PlayFrostClips();

        yield return coldOrbDuration;

        //isTargeted = false;

        coldOrb.GetComponent<ColdSnapTrigger>().Die();
        //Destroy(coldOrb, 1f);
        //isAttacking = false;

    }

    //LIGHTNINGSTRIKE POWER
    public IEnumerator animateLightningStrike()
    {
        

        //Attacking animation of player
        SetCastAnimation();

        // Move our position a step closer to the target.
        //var step = 5 * Time.deltaTime; // calculate distance to move

        /*
        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {

            clone.transform.position = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y + 20, enemyTarget.transform.position.z);

            LightningAttack();

        }
        */
        InitializeLightningStrikeTrig();

        /*
        if (puzzleTarget != null && puzzleTarget.GetComponentInParent<ShutOffTerminal>())
        {
            puzzleTarget.GetComponentInParent<ShutOffTerminal>().TerminalShutOff();
        }
        */

        //Stop action during the course of animation and yield time
        c_soundLibrary.PlayLightningClips();

        yield return lightningDuration;

        lightning.GetComponent<LightningTrigger>().Die();
        //isAttacking = false;
    }

    //MOONTIDE POWER
    public IEnumerator animateMoonTide()
    {

        InitializeMoonTide();

        //Attacking animation of player
        SetCastAnimation();

        //If enemy is around make the cold orb target said enemy >> update 
        /*
        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {
            isTargeted = true;
            MoonTideAttack();
        }
        */

        c_soundLibrary.PlayWaveClips();
        yield return waveDuration;

        //reset animation, is attacking orb target, detroy te orb gameobject and reset DPAD
        //isTargeted = false;
        moonTide.GetComponent<MoontideTrigger>().Die();
        //Destroy(coldOrb, 1f);
        //isAttacking = false;
    }

    //Turns on the general cast animation on Celeste
    public void SetCastAnimation()
    {
        //Attacking animation of player
        animator.SetAnimationFlag("cast", true);
        SuspendActions(true);
    }


    /// <summary>
    /// HANDLES DEALING DAMAGE WITH EACH POWER
    /// </summary>
    public void ColdSnapAttack(Enemy enemy)
    {
        bool enemyIsDead;
        //if (enemy && powerInUse == Power.COLDSNAP && canColdSnap == true)
        if (enemy)
        {
            Power weakness = GetEnemyWeakness(enemy);
            int HitPoints = GetPowerHitDamage(Power.COLDSNAP, weakness);
            enemyIsDead = enemy.TakeHit(HitPoints);
        }
    }

    public void LightningAttack(Enemy enemy)
    {
        bool enemyIsDead;
        //if (enemy && powerInUse == Power.LIGHTNINGSTRIKE && canLightningStrike == true)
        if (enemy)
        {
            Power weakness = GetEnemyWeakness(enemy);
            int HitPoints = GetPowerHitDamage(Power.LIGHTNINGSTRIKE, weakness);
            enemyIsDead = enemy.TakeHit(HitPoints);
        }
    }

    public void MoonTideAttack(Enemy enemy)
    {
        bool enemyIsDead;
        //if (enemy && powerInUse == Power.MOONTIDE && canMoonTide == true)
        if (enemy)
        {
            Power weakness = GetEnemyWeakness(enemy);
            int HitPoints = GetPowerHitDamage(Power.MOONTIDE, weakness);
            enemyIsDead = enemy.TakeHit(HitPoints);
        }
    }

    

    ///
    /// RESETS ABILITIES AFTER THEY ARE USED, STARTS COOLDOWNS
    ///
    public void ResetColdSnap()
    {
        powerInUse = Power.NONE;
        float coldSnapTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats);

        //Start our cooldowns
        StartCoroutine(ResetCooldownTime(coldSnapTimer, powerBehaviour.ColdSnapStats));
        StartCooldownUI("CastCold", coldSnapTimer);
    }

    public void ResetLightningStrike()
    {
        powerInUse = Power.NONE;
        float lightningStrikeTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats);

        //Start our cooldowns
        StartCoroutine(ResetCooldownTime(lightningStrikeTimer, powerBehaviour.LightningStats));
        StartCooldownUI("CastThunder", lightningStrikeTimer);
    }

    public void ResetBasic()
    {
        powerInUse = Power.NONE;
        float basicAttackTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats);

        //Start our cooldowns
        StartCoroutine(ResetCooldownTime(basicAttackTimer, powerBehaviour.BasicAttackStats));
        StartCooldownUI("BasicAttack", basicAttackTimer);
    }

    public void ResetMoonTide()
    {
        powerInUse = Power.NONE;
        float moonTideTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats);

        //Start our cooldowns
        StartCoroutine(ResetCooldownTime(moonTideTimer, powerBehaviour.MoonTideAttackStats));
        StartCooldownUI("CastMoontide", moonTideTimer);
    }


    ///
    /// INITIALIZE EACH POWER THAT CREATES AN EFFECT HERE
    ///

    private void InitializeColdSnap()
    {
        //Get our spawn position
        Vector3 spawnPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z);
        Quaternion lookRot = GetComponent<CelestialPlayerMovement>().GetPlayerObj().localRotation;

        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        coldOrb = Instantiate((powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualGameObj), 
            spawnPos, lookRot);
        coldOrb.GetComponent<ColdSnapTrigger>().SetPlayer(this);
        coldOrb.GetComponent<ColdSnapTrigger>().InitializeSelf(GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward);
    }

    //Create and set the position and rotation of the physical wave object in MoonTide
    private void InitializeMoonTide()
    {
        //Set up the rotation we will use for the wave's orientation
        Quaternion lookRot = new Quaternion();
        //Get the actual angle, set to degrees instead of radians
        float theta = Mathf.Atan2(-GetComponent<CelestialPlayerMovement>().GetPlayerObj().right.x, -GetComponent<CelestialPlayerMovement>().GetPlayerObj().right.z) * Mathf.Rad2Deg;
        //Set our y rotation to this, leave the others blank
        Vector3 lookAngle = new Vector3(0, theta, 0);
        lookRot.eulerAngles = lookAngle;

        //To get an offset posiiton relative to our facing direction, multiply those vectors together
        Vector3 spawnVector = (this.transform.position - (this.transform.position + moontideOffset)).normalized;
        //Vector3 spawnVector = new Vector3(moontideOffset.x / GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward.x,
        //    0f, moontideOffset.z / GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward.z).normalized;
        //Vector3 spawnPos = Vector3.Scale(spawnVector, (GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward));
        //Vector3 spawnPos = (spawnVector - GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward).normalized;
        float theta2 = Mathf.Atan2(GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward.x, GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward.z);
        Vector3 lookAngle2 = new Vector3(0, theta, 0);
        Vector3 spawnPos = Quaternion.Euler(lookAngle2) * spawnVector;
        //Then scale that vector based on how far we wanted the object
        //spawnPos = Vector3.Scale(moontideOffset, spawnPos);
        spawnPos *= Mathf.Abs(moontideOffset.magnitude);

        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        moonTide = Instantiate(powerBehaviour.GetComponent<PowerBehaviour>().MoonTideAttackStats.visualGameObj,
            (this.transform.position + spawnPos), lookRot);
        moonTide.GetComponent<MoontideTrigger>().SetPlayer(this);
        moonTide.GetComponent<MoontideTrigger>().InitializeSelf(-GetComponent<CelestialPlayerMovement>().GetPlayerObj().right);
    }

    //Create and set the position and rotation of the physical lightning object in LightningStrike
    private void InitializeLightningStrikeTrig()
    {
        validTargets.Clear();

        //Grab a list of all enemies within range distance of the lightning strike
        foreach(GameObject enemy in enemyList.Items)
        {
            if(JudgeDistance(enemy.transform.position, this.transform.position, spellRange))
            {
                validTargets.Add(enemy.gameObject);
            }
        }
        if(validTargets.Count > 0)
        {
            //We want to clear out any enemies that are not inside the angle in front of Celeste
            List<GameObject> enemiesToRemove = new List<GameObject>();
            foreach(GameObject enemy in validTargets)
            {
                Vector3 directionToTarget = (enemy.transform.position - this.transform.position).normalized;
                if(Vector3.Angle(transform.forward, directionToTarget) !< lightningAngle / 2)
                {
                    enemiesToRemove.Add(enemy);
                }
            }
            //We do this on a separate iteration because otherwise Unity gets cranky
            foreach(GameObject enemy in enemiesToRemove)
            {
                validTargets.Remove(enemy);
            }
            
            //If there's at least one enemy in range in front of us, we pick the closest enemy as our power target
            if(validTargets.Count > 0)
            {
                PickClosestTarget();
                lightning = Instantiate(powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualGameObj,
            new Vector3(powerTarget.transform.position.x, powerTarget.transform.position.y, powerTarget.transform.position.z), Quaternion.identity);
            }
            else
            {
                Vector3 spawnPos = -GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward * lightningOffset;

                lightning = Instantiate(powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualGameObj, (this.transform.position - spawnPos), Quaternion.identity);
            }
        }
        else
        {
            Vector3 spawnPos = -GetComponent<CelestialPlayerMovement>().GetPlayerObj().forward * lightningOffset;

            lightning = Instantiate(powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualGameObj, (this.transform.position - spawnPos), Quaternion.identity);
        }

        
        lightning.GetComponent<LightningTrigger>().SetPlayer(this);
        //lightning.GetComponent<LightningTrigger>().InitializeSelf();
        //VisualEffect lightningStrike = powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualDisplay;
        //VisualEffect clone = Instantiate(lightningStrike, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        //lightning.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
    }



    /// <summary>
    /// ATTACK HELPERS
    /// </summary>

    //Handles calculating the final damage dealt by a particular power
    public int GetPowerHitDamage(Power powerUsed, Power weakness)
    {
        PowerBehaviour attack;
        attack = GetComponent<PowerBehaviour>();
        int powerDamage = 0;
        if (powerUsed == Power.BASIC)
        {
            powerDamage = Random.Range(attack.BasicAttackStats.minDamage, attack.BasicAttackStats.maxDamage);

            return powerDamage;
        }


        if (powerUsed == Power.COLDSNAP)
        {

            if (weakness == Power.COLDSNAP)
            {
                powerDamage = attack.ColdSnapStats.maxDamage;
            }
            else if (weakness != Power.COLDSNAP)
            {
                powerDamage = attack.ColdSnapStats.minDamage;

            }
            return powerDamage;
        }

        if (powerUsed == Power.MOONTIDE)
        {
            //DrainEnergy(attack.MoonTideAttackStats.energyDrain);
            if (weakness == Power.MOONTIDE)
            {
                powerDamage = attack.MoonTideAttackStats.maxDamage;
            }
            else if (weakness != Power.MOONTIDE)
            {
                powerDamage = attack.MoonTideAttackStats.minDamage;

            }

            return powerDamage;
        }


        if (powerUsed == Power.LIGHTNINGSTRIKE)
        {
            //DrainEnergy(attack.LightningStats.energyDrain);
            if (weakness == Power.LIGHTNINGSTRIKE)
            {
                powerDamage = attack.LightningStats.maxDamage;
            }
            else if (weakness != Power.LIGHTNINGSTRIKE)
            {
                powerDamage = attack.LightningStats.minDamage;

            }

            return powerDamage;
        }
        return powerDamage;
    }

    //Returns which power an enemy is weak to
    public Power GetEnemyWeakness(Enemy enemyTarget)
    {
        if (enemyTarget.GetEnemyStats().enemyType == EnemyStats.enemyTypes.OilMonster)
        {
            return Power.COLDSNAP;
        }

        if (enemyTarget.GetEnemyStats().enemyType == EnemyStats.enemyTypes.Smog)
        {
            return Power.MOONTIDE;
        }
        return Power.NONE;

    }

    //Handles moving the ColdSnap orb forward from Celeste
    /*
    public void ShootTowardsTarget(GameObject Orb)
    {
        var step = 20 * Time.deltaTime; // calculate distance to move
        Orb.transform.position = Vector3.MoveTowards(Orb.transform.position, enemyTarget.transform.position, step);
        Orb.transform.LookAt(enemyTarget.transform.position, Vector3.up);

    }
    */

    //Use this to handle moving the moontide attack after it's been summoned
    public void HandleWaveMove()
    {

    }

    //Turns a cooldown on and then off again after it is used
    public IEnumerator ResetCooldownTime(float timer, PowerStats powerStats)
    {
        powerStats.isOnCooldown = true;

        yield return new WaitForSeconds(timer);

        powerStats.isOnCooldown = false;
        if(powerStats.powerType == "BasicAttack")
            canBasicAttack = true;
        if (powerStats.powerType == "ColdSnap")
            canColdSnap = true;
        if (powerStats.powerType == "Lightning")
            canLightningStrike = true;
        if (powerStats.powerType == "MoonTidePowerStats")
            canMoonTide = true;
        OnPowerStateChange();
    }

    public void ClearColdSnap()
    {
        coldOrb = null;
    }

    public void ClearMoonTide()
    {
        moonTide = null;
    }

    public void ClearLightning()
    {
        lightning = null;
    }



    /// <summary>
    /// OTHER ABILITY MAIN FUNCTIONS
    /// </summary>
    /// <returns></returns>

    //Turns on Celeste's dodge animation and tells the movement script to handle moving her
    private void SetDodge()
    {
        animator.SetAnimationFlag("dodge", true);
        StartDodgeMovement();
        SuspendActions(true);
    }



    /// <summary>
    /// HUD HELPER FUNCTIONS
    /// </summary>
    /// <returns></returns>
    //Turns off Celeste's controls and updates her UI to match
    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        celestialControls.controls.CelestialPlayerDefault.Disable();
        hudManager.SetCelesteOccupied(true);
        OnPowerStateChange();

        yield return waitTime;

        celestialControls.controls.CelestialPlayerDefault.Enable();
        hudManager.SetCelesteOccupied(false);
        OnPowerStateChange();
    }

    //Toggles whether actions are suspended or not for no defined time
    public override void SuspendActions(bool suspend)
    {
        if (suspend)
        {
            celestialControls.controls.CelestialPlayerDefault.Disable();
            hudManager.SetCelesteOccupied(true);
        }
        else
        {
            celestialControls.controls.CelestialPlayerDefault.Enable();
            hudManager.SetCelesteOccupied(false);
        }
        
        OnPowerStateChange();
    }



    /// <summary>
    /// Checks all parameters required for Celeste to be able to cast a given spell
    /// </summary>
    /// <param name="power"> The power we're checking if can be cast </param>
    /// <param name="checking"> Whether we're casting the spell or only checking if it's castable </param>
    /// <returns></returns>
    public bool CheckIfCastable(PowerStats power, bool checking)
    {
        if (PowerNotEnabled(power, checking))
            return false;
        if (NotEnoughEnergy(power, checking))
            return false;
        if (PowerOnCooldown(power, checking))
            return false;

        //If none of these checks fail, we're good to go!
        return true;
    }

    //Returns true if the power hasn't been unlocked
    public bool PowerNotEnabled(PowerStats power, bool checking)
    {
        if (!power.isEnabled)
        {
            string enPowerNotEnabled = "Power hasn't been unlocked";
            string frPowerNotEnabled = "L'alimentation n'a pas été déverrouillée";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if (!checking)
                hudManager.ThrowPlayerWarning(enPowerNotEnabled, frPowerNotEnabled);
            return true;
        }
        return false;
    }

    //Returns true if the power costs more energy than the player has
    public bool NotEnoughEnergy(PowerStats power, bool checking)
    {
        if (energy.current <= power.energyDrain)
        {
            string enNotEnoughEnergy = "Not enough energy";
            string frNotEnoughEnergy = "Pas assez d'énergie";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if (!checking)
                hudManager.ThrowPlayerWarning(enNotEnoughEnergy, frNotEnoughEnergy);
            return true;
        }
        return false;
    }

    //Used specifically for rain, which doesn't have a stat block
    public bool NotEnoughEnergy(int powerCost, bool checking)
    {
        if (energy.current < powerCost)
        {
            string enNotEnoughEnergy = "Not enough energy";
            string frNotEnoughEnergy = "Pas assez d'énergie";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if (!checking)
                hudManager.ThrowPlayerWarning(enNotEnoughEnergy, frNotEnoughEnergy);
            return true;
        }
        return false;
    }

    //Returns true if the power is on cooldown
    public bool PowerOnCooldown(PowerStats power, bool checking)
    {
        if (power.isOnCooldown)
        {
            string enPowerOnCooldown = "Power is still on cooldown";
            string frPowerOnCooldown = "La puissance est toujours en recharge";
            if (!checking)
                hudManager.ThrowPlayerWarning(enPowerOnCooldown, frPowerOnCooldown);
            return true;
        }
        return false;
    }



    /// <summary>
    /// ANIMATION EVENT HANDLERS
    /// </summary>
    /// <returns></returns>

    //Turns on the Celestial player's physical movement while dodging
    public void StartDodgeMovement()
    {
        //This will be turned off by the animator FSM or animation event
        //isDodging = true;
        this.GetComponent<CelestialPlayerMovement>().ToggleDodging(true);
    }

    //Turns off the Celestial player's physical movement while dodging
    public void StopDodgeMovement()
    {
        //isDodging = false;
        this.GetComponent<CelestialPlayerMovement>().ToggleDodging(false);
    }

    //This makes Celeste unable to take hits from enemies
    public void StartIFrames()
    {
        iFramesOn = true;
    }

    //This makes Celeste vulnerable to enemy attacks again
    public void EndIFrames()
    {
        iFramesOn = false;
    }

    public void AttackCollisionOn()
    {
        staff.ToggleAttacking(true);
    }

    public void AttackCollisionOff()
    {
        staff.ToggleAttacking(false);
    }



    /// <summary>
    /// ENERGY FUNCTIONS
    /// </summary>

    //Handles increases in energy as from energy drops
    public void IncreaseEnergy(int pointsAdded)
    {
        hudManager.InstantiateEnergyIcon(pointsAdded);
        energy.current += pointsAdded;
        OnPowerStateChange();
        if (OnEnergyChanged != null)
            OnEnergyChanged(energy.current, energy.max);
    }

    public IEnumerator RegenEnergy()
    {
        while (true)
        {
            yield return energyRegenTimer;

            if (energy.current < 5)
            {
                energy.current++;
                OnPowerStateChange();
                if (OnEnergyChanged != null)
                    OnEnergyChanged(energy.current, energy.max);
            }
        }
        
    }
    

    //Handles draining energy 1 time for spells
    public void DrainEnergy(int pointsDrained)
    {
        energy.current -= Mathf.Clamp(pointsDrained, 0, energy.max);
        if (CheckIfEnergyDrop(energy.current + pointsDrained, pointsDrained))
            OnPowerStateChange();
        if (OnEnergyChanged != null)
            OnEnergyChanged(energy.current, energy.max);
    }

    //Handles draining energy for rain
    public IEnumerator DrainRainEnergy()
    {
        int rainDrain = 1;

        // Gradually decrease fill amount over cooldown duration
        while (isRaining)
        {
            if (energy.current > 0)
            // Find the energy bar fill Image component dynamically
            {
                DrainEnergy(rainDrain);
            }
            else
            {
                isRaining = false;
                weatherState.SetRainyState(false);
                RainParticleSystem.SetActive(false);

                NotEnoughEnergy(rainDrain, false);
            }

            yield return new WaitForSeconds(1);

        }
    }

    //Returns true if the current energy drain would make a power unusable
    private bool CheckIfEnergyDrop(int currentEnergy, int energyDrain)
    {
        if ((currentEnergy - energyDrain < 1) && currentEnergy >= 1)
        {
            return true;
        }
        if ((currentEnergy - energyDrain < powerBehaviour.ColdSnapStats.energyDrain) && currentEnergy >= powerBehaviour.ColdSnapStats.energyDrain)
        {
            return true;
        }
        if ((currentEnergy - energyDrain < powerBehaviour.LightningStats.energyDrain) && currentEnergy >= powerBehaviour.LightningStats.energyDrain)
        {
            return true;
        }
        if ((currentEnergy - energyDrain < powerBehaviour.MoonTideAttackStats.energyDrain) && currentEnergy >= powerBehaviour.MoonTideAttackStats.energyDrain)
        {
            return true;
        }
        return false;
    }

    public void OnMenuOpen()
    {
        hudManager.ToggleMenuPanel(true);
    }

    public void OnMenuClose()
    {
        hudManager.ToggleMenuPanel(false);
    }

    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>
    /// <returns></returns>

    public float GetEnergy()
    {
        return energy.current;
    }

    public int GetMaxEnergy()
    {
        return maxEnergyVal;
    }

    public int GetStartingEnergy()
    {
        return startingEnergyVal;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    public bool GetCanBasicAttack()
    {
        return canBasicAttack;
    }

    public void SetCanBasicAttack(bool canDo)
    {
        canBasicAttack = canDo;
    }

    public bool GetCanColdSnap()
    {
        return canColdSnap;
    }

    public void SetCanColdSnap(bool canDo)
    {
        canColdSnap = canDo;
    }

    public bool GetCanLightningStrike()
    {
        return canLightningStrike;
    }

    public void SetCanLightningStrike(bool canDo)
    {
        canLightningStrike = canDo;
    }

    public bool GetCanMoonTide()
    {
        return canMoonTide;
    }

    public void SetCanMoonTide(bool canDo)
    {
        canMoonTide = canDo;
    }

    public bool GetIsRaining()
    {
        return isRaining;
    }

    public void SetIsRaining(bool raining)
    {
        isRaining = raining;
    }

    public bool GetTriggeredRain()
    {
        return triggeredRain;
    }

    public void SetRainTriggered(bool triggered)
    {
        triggeredRain = triggered;
    }
}
