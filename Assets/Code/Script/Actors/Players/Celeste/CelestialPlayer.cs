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

    //private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    private bool isRaining = false;
    private bool triggeredRain = false;
    private bool isDodging = false;
    [SerializeField] public GameObject RainParticleSystem;

    //[Header("Button press")]
    /*
    private bool buttonRain = false;
    private bool buttonBasicAttack = false;
    private bool buttonColdSnap = false;
    private bool buttonLightningStrike = false;
    private bool buttonMoonTide = false;
    */

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

    private WaitForSeconds basicCoolDownTime;
    private WaitForSeconds coldSnapCoolDownTime;
    private WaitForSeconds lightningCoolDownTime;

    private WaitForSeconds dodgeMoveStopTime;

    private bool isTargeted = false;

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

    public event System.Action<int, int> OnEnergyChanged;
    public event System.Action OnPowerStateChange;
    //public event System.Action<string, float> OnCooldownStarted;

    public Stat energy;
    PowerBehaviour powerBehaviour;
    public CelestialPlayerControls celestialControls;

    [Header("Power Drop Assets")]
    private VisualEffect powerDrop;
    GameObject coldOrb;
    GameObject moonTide;

    [SerializeField] private CelesteSoundLibrary c_soundLibrary;

    [SerializeField] public GameObject treeSeedPrefab;
    //private CelestialPlayerInputActions celestialPlayerInput;
    private PlayerInput playerInput;
    // [Header("Lightning System")]
    // Start is called before the first frame update

    //Interaction with the player
    public bool enemySeen = false;
    public bool enemyHit = false;
    public GameObject enemyTarget = null;
    public Vector3 enemyLocation;

    //private bool inRangeOfPuzzle = false;

    private void Awake()
    {
        OrigPos = this.gameObject.transform.position;
        animator = GetComponent<CelestialPlayerAnimator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        celestialControls = GetComponent<CelestialPlayerControls>();
        health = new Stat(100, 100, false);
        energy = new Stat(100, 50, true);
        //uiManager = GetComponent<CelestUIManager>();
        c_soundLibrary = base.soundLibrary as CelesteSoundLibrary;
    }

    void Start()
    {
        dodgeMoveStopTime = new WaitForSeconds(0.7f);
        powerBehaviour = GetComponent<PowerBehaviour>();
        
        //dodgeAnimTime = new WaitForSeconds();
        coldSnapCoolDownTime = powerBehaviour.ColdSnapStats.rechargeTimer;
        basicCoolDownTime = powerBehaviour.BasicAttackStats.rechargeTimer;
        lightningCoolDownTime = powerBehaviour.LightningStats.rechargeTimer;
        staff = GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();
        staff.SetPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeted)
        {
            ShootTowardsTarget(coldOrb);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = true;

            enemyLocation = other.transform.position;
            enemyTarget = other.transform.gameObject;

        }
        if (other.GetComponent<ClearDebrisTrigger>())
        {
            //inRangeOfPuzzle = true;
            enemyTarget = other.transform.gameObject;
        }
        else if (other.GetComponent<ShutOffTerminal>())
        {
            //inRangeOfPuzzle = true;
            enemyTarget = other.GetComponent<ShutOffTerminal>().GetStrikeTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "Enemy")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = false;

            enemyLocation = other.transform.position;
            enemyTarget = null;
        }
        if (other.GetComponent<ClearDebrisTrigger>() || other.GetComponent<ShutOffTerminal>())
        {
            //inRangeOfPuzzle = false;
            enemyTarget = null;
        }
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
        StartCoroutine(AnimateDodge());
    }


    //POWER ANIMATION
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
    /// Celestial Player Control >> Powers are animated>> Heads into Reseting the Power 
    /// 

    //Runs the basic attack after the button has been pressed
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

    //COLDSNAP POWER
    public IEnumerator animateColdSnap()
    {
        
        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        coldOrb = Instantiate((powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualGameObj), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);

        //Attacking animation of player
        animator.SetAnimationFlag("cast", true);

        //If enemy is around make the cold orb target said enemy >> update 
        if (enemyTarget != null)
        {
            isTargeted = true;
            ColdSnapAttack();
        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("cast")));
        c_soundLibrary.PlayFrostClips();
        yield return animator.GetAnimationWaitTime("cast");

        //reset animation, is attacking orb target, detroy te orb gameobject and reset DPAD
        //celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
        animator.SetAnimationFlag("cast", false);
        isTargeted = false;
        Destroy(coldOrb, 1f);
        //ResetImageColor(celestPlayerDpad); //reset dpad colors
        isAttacking = false;

    }

    //LIGHTNINGSTRIKE POWER
    //TO BE CHANGED!!!!!!!
    public IEnumerator animateLightningStrike()
    {
        VisualEffect lightningStrike = powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualDisplay;
        VisualEffect clone = Instantiate(lightningStrike, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        clone.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);

        //Attacking animation of player
        animator.SetAnimationFlag("cast", true);

        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {

            clone.transform.position = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y + 20, enemyTarget.transform.position.z);

            LightningAttack();

        }
        if (enemyTarget != null && enemyTarget.GetComponentInParent<ShutOffTerminal>())
        {
            enemyTarget.GetComponentInParent<ShutOffTerminal>().TerminalShutOff();
        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("cast")));
        c_soundLibrary.PlayLightningClips();
        yield return animator.GetAnimationWaitTime("cast");

        //reset animation, reset isattacking, detroy visual asset and reset DPAD
        animator.SetAnimationFlag("cast", false);
        Destroy(clone, 1f);
        isAttacking = false;
    }

    //MOONTIDE POWER
    public IEnumerator animateMoonTide()
    {
        
        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        coldOrb = Instantiate((powerBehaviour.GetComponent<PowerBehaviour>().MoonTideAttackStats.visualGameObj), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        //Attacking animation of player
        animator.SetAnimationFlag("cast", true);

        //If enemy is around make the cold orb target said enemy >> update 
        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {
            isTargeted = true;
            MoonTideAttack();
        }
        if (enemyTarget != null && enemyTarget.GetComponent<ClearDebrisTrigger>())
        {
            enemyTarget.GetComponent<ClearDebrisTrigger>().InitiateClear();
        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("cast")));
        c_soundLibrary.PlayWaveClips();
        yield return animator.GetAnimationWaitTime("cast");

        //reset animation, is attacking orb target, detroy te orb gameobject and reset DPAD
        animator.SetAnimationFlag("cast", false);
        isTargeted = false;
        Destroy(coldOrb, 1f);
        isAttacking = false;
    }


    /// <summary>
    /// HANDLES DEALING DAMAGE WITH EACH POWER
    /// </summary>
    public void ColdSnapAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.COLDSNAP && canColdSnap == true)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }

    /*
    public void BasicAttack()
    {
        bool enemyIsDead;
        //if (enemyTarget && powerInUse == Power.BASIC && canBasicAttack == false)
        if ( powerInUse == Power.BASIC && canBasicAttack == true)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }
    */

    public void LightningAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.LIGHTNINGSTRIKE && canLightningStrike == true)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }

    public void MoonTideAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.MOONTIDE && canMoonTide == true)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
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



    /// <summary>
    /// ATTACK HELPERS
    /// </summary>
    
    //Handles calculating the final damage dealt by a particular power
    public int GetPowerHitDamage(Power weakness)
    {
        PowerBehaviour attack;
        attack = GetComponent<PowerBehaviour>();
        int powerDamage = 0;
        if (powerInUse == Power.BASIC)
        {
            powerDamage = Random.Range(attack.BasicAttackStats.minDamage, attack.BasicAttackStats.maxDamage);

            return powerDamage;
        }


        if (powerInUse == Power.COLDSNAP)
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

        if (powerInUse == Power.MOONTIDE)
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


        if (powerInUse == Power.LIGHTNINGSTRIKE)
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
        return 0;


    }

    //Returns which power an enemy is weak to
    public Power GetEnemyWeakness(GameObject enemyTarget)
    {
        if (enemyTarget.GetComponent<Enemy>().GetEnemyStats().enemyType == EnemyStats.enemyTypes.OilMonster)
        {
            return Power.COLDSNAP;
        }

        if (enemyTarget.GetComponent<Enemy>().GetEnemyStats().enemyType == EnemyStats.enemyTypes.Smog)
        {
            return Power.MOONTIDE;
        }
        return Power.NONE;

    }

    //Handles moving the ColdSnap orb forward from Celeste
    public void ShootTowardsTarget(GameObject Orb)
    {
        var step = 20 * Time.deltaTime; // calculate distance to move
        Orb.transform.position = Vector3.MoveTowards(Orb.transform.position, enemyTarget.transform.position, step);
        Orb.transform.LookAt(enemyTarget.transform.position, Vector3.up);

    }

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


    /// <summary>
    /// OTHER ABILITY MAIN FUNCTIONS
    /// </summary>
    /// <returns></returns>

    //Runs the animation flags for dodging, tells movement script to move the character
    private IEnumerator AnimateDodge()
    {
        animator.SetAnimationFlag("dodge", true);
        StartDodgeMovement();
        StartCoroutine(SuspendActions(animator.GetAnimationWaitTime("dodge")));

        yield return animator.GetAnimationWaitTime("dodge");

        animator.SetAnimationFlag("dodge", false);
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

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange)
    {
        yield return waitTime;
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
            string powerNotEnabled = "Power hasn't been unlocked";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if (!checking)
                hudManager.ThrowPlayerWarning(powerNotEnabled);
            return true;
        }
        return false;
    }

    //Returns true if the power costs more energy than the player has
    public bool NotEnoughEnergy(PowerStats power, bool checking)
    {
        if (energy.current <= power.energyDrain)
        {
            string notEnoughEnergy = "Not enough energy";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if(!checking)
                hudManager.ThrowPlayerWarning(notEnoughEnergy);
            return true;
        }
        return false;
    }

    //Used specifically for rain, which doesn't have a stat block
    public bool NotEnoughEnergy(int powerCost, bool checking)
    {
        if (energy.current < powerCost)
        {
            string notEnoughEnergy = "Not enough energy";
            //Allows a use case for the system to call this function even if it's not intending to use the ability
            if (!checking)
                hudManager.ThrowPlayerWarning(notEnoughEnergy);
            return true;
        }
        return false;
    }

    //Returns true if the power is on cooldown
    public bool PowerOnCooldown(PowerStats power, bool checking)
    {
        if (power.isOnCooldown)
        {
            string powerOnCooldown = "Power is still on cooldown";
            if (!checking)
                hudManager.ThrowPlayerWarning(powerOnCooldown);
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
        isDodging = true;
        this.GetComponent<CelestialPlayerMovement>().ToggleDodging(true);
    }

    //Turns off the Celestial player's physical movement while dodging
    public void StopDodgeMovement()
    {
        isDodging = false;
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
        energy.current += pointsAdded;
        OnPowerStateChange();
        if (OnEnergyChanged != null)
            OnEnergyChanged(energy.current, energy.max);
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



    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>
    /// <returns></returns>

    public float GetEnergy()
    {
        return energy.current;
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
