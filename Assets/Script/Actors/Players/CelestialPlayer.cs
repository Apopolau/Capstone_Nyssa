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

    private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    [SerializeField] public bool isRaining = false;
    private bool isDodging = false;
    [SerializeField] public GameObject RainParticleSystem;

    [Header("Button press")]
    [SerializeField] public bool buttonRain = false;
    [SerializeField] public bool buttonBasicAttack = false;
    [SerializeField] public bool buttonColdSnap = false;
    [SerializeField] public bool buttonLightningStrike = false;
    [SerializeField] public bool buttonMoonTide = false;

    [Header("Attack")]
    CelestialPlayerBasicAttackTrigger staff;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool canBasicAttack = true;
    [SerializeField] public bool canColdSnap = true;
    [SerializeField] public bool canLightningStrike = true;
    [SerializeField] public bool canMoonTide = true;
    [SerializeField] public bool canSetFogTrap = true;
    [SerializeField] public bool canSetFrostTrap = true;
    [SerializeField] public bool canSunBeam = true;

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

    //Lengths of animations and abilities
    private WaitForSeconds dodgeAnimTime;
    private WaitForSeconds dodgeMoveStopTime;
    private WaitForSeconds specialPowerAnimTime;
    private WaitForSeconds basicPowerAnimTime;
    private WaitForSeconds basicCoolDownTime;
    private WaitForSeconds coldSnapCoolDownTime;
    private WaitForSeconds lightningCoolDownTime;

    [Header("Animation")]
    private CelestialPlayerAnimator celestialAnimator;

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

    private bool inRangeOfPuzzle = false;

    private void Awake()
    {
        OrigPos = this.transform.position;
        celestialAnimator = GetComponent<CelestialPlayerAnimator>();
        celestialAgent = GetComponent<NavMeshAgent>();
        celestialAgent.enabled = false;
        celestialControls = GetComponent<CelestialPlayerControls>();
        health = new Stat(100, 100, false);
        energy = new Stat(100, 50, true);
        //uiManager = GetComponent<CelestUIManager>();
        c_soundLibrary = base.soundLibrary as CelesteSoundLibrary;
        
        //virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }


    void Start()
    {
        dodgeAnimTime = new WaitForSeconds(1.290f);
        dodgeMoveStopTime = new WaitForSeconds(0.7f);
        powerBehaviour = GetComponent<PowerBehaviour>();
        basicPowerAnimTime = new WaitForSeconds(1.208f);
        specialPowerAnimTime = new WaitForSeconds(1.958f);
        //dodgeAnimTime = new WaitForSeconds();
        coldSnapCoolDownTime = powerBehaviour.ColdSnapStats.rechargeTimer;
        basicCoolDownTime = powerBehaviour.BasicAttackStats.rechargeTimer;
        lightningCoolDownTime = powerBehaviour.LightningStats.rechargeTimer;
        staff = GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();
        staff.SetPlayer(this);


        //StartCoroutine(CoolDownImageFill(lightingStrikeFill));
        //StartCoroutine(CoolDownImageFill(CTRLightingStrikeFill));

        //rainFill.enabled = false;
        //CTRLRainFill.enabled = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (isTargeted)
        {
            ShootTowardsTarget(coldOrb);
        }
        /*
        if (isDodging)
        {
            //HandlePlayerDodge();
        }
        */

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            interacting = false;
        }
    }

    //When the player presses the dodge button
    public void OnDodgeSelected(InputAction.CallbackContext context)
    {
        //this.GetComponent<CelestialPlayerMovement>().Dodge();
        StartCoroutine(StopDodgeMovement());
        StartCoroutine(AnimateDodge());
        StartCoroutine(SuspendActions(dodgeAnimTime));
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
            inRangeOfPuzzle = true;
            enemyTarget = other.transform.gameObject;
        }
        else if (other.GetComponent<ShutOffTerminal>())
        {
            inRangeOfPuzzle = true;
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
            inRangeOfPuzzle = false;
            enemyTarget = null;
        }
    }

    //POWER BUTTONS
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    /// Celestial Player Control >> Power buttons are selected >> Heads int FSM CAN_____powersname____
    /// 
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
            }
            //We could replace this with some other effect to show this is turned on?
            //rainFill.enabled = true;
            //CTRLRainFill.enabled = true;
        }
        else if (isRaining)
        {
            // yield return new WaitForSeconds(10f);

            weatherState.SetRainyState(false);

            isRaining = false;
            //rainFill.enabled = false;
            //CTRLRainFill.enabled = false;

        }
        buttonRain = true;

    }



    //POWEER ANIMATION
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
    /// Celestial Player Control >> Powers are animated>> Heads into Reseting the Power 
    /// 
    public IEnumerator animateColdSnap()
    {
        //COLDSNAP POWER
        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        coldOrb = Instantiate((powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualGameObj), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);

        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);

        //If enemy is around make the cold orb target said enemy >> update 
        if (enemyTarget != null)
        {
            isTargeted = true;
            ColdSnapAttack();
        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(specialPowerAnimTime));
        c_soundLibrary.PlayFrostClips();
        yield return specialPowerAnimTime;

        //reset animation, is attacking orb target, detroy te orb gameobject and reset DPAD
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
        isTargeted = false;
        Destroy(coldOrb, 1f);
        //ResetImageColor(celestPlayerDpad); //reset dpad colors
        isAttacking = false;

    }

    public IEnumerator animateBasicAttack()
    {
        //BASIC ATTACK
        //TO BE CHANGED!!!!!!!

        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);

        //If enemy is around call the basic attack
        //if (enemyTarget != null && staff.enemyHit)
        /*
        if (staff.enemyHit)
        {
            staff.BasicAttack(powerBehaviour.GetComponent<PowerBehaviour>().BasicAttackStats);
           //BasicAttack();

        }
        */
        //Debug.Log("Animating basic attack");
        staff.ToggleAttacking(true);

        StartCoroutine(SuspendActions(basicPowerAnimTime));
        c_soundLibrary.PlayAttackClips();
        yield return basicPowerAnimTime;
        staff.ToggleAttacking(false);
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, false);
        //ResetImageColor(celestPlayerDpad); //reset dpad colors
        isAttacking = false;

    }

    public IEnumerator animateLightningStrike()
    {

        //LIGHTNINGSTRIKE POWER
        //TO BE CHANGED!!!!!!!

        VisualEffect lightningStrike = powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualDisplay;
        VisualEffect clone = Instantiate(lightningStrike, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        clone.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);

        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);


        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {

            clone.transform.position = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y + 20, enemyTarget.transform.position.z);

            LightningAttack();

        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(specialPowerAnimTime));
        c_soundLibrary.PlayLightningClips();
        yield return specialPowerAnimTime;

        if (enemyTarget != null && enemyTarget.GetComponentInParent<ShutOffTerminal>())
        {
            enemyTarget.GetComponentInParent<ShutOffTerminal>().TerminalShutOff();
        }

        //reset animation, reset isattacking, detroy visual asset and reset DPAD
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
        Destroy(clone, 1f);
        //ResetImageColor(celestPlayerDpad); //reset dpad colors
        isAttacking = false;

    }

    public IEnumerator animateMoonTide()
    {
        //MOONTIDE POWER
        //Instatiate the visual asset and set it to the ColdOrB game object, spawn the cold orb at the player
        coldOrb = Instantiate((powerBehaviour.GetComponent<PowerBehaviour>().MoonTideAttackStats.visualGameObj), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);

        //If enemy is around make the cold orb target said enemy >> update 
        if (enemyTarget != null && enemyTarget.GetComponent<Enemy>())
        {
            isTargeted = true;
            MoonTideAttack();
        }

        //Stop action during the course of animation and yield time
        StartCoroutine(SuspendActions(specialPowerAnimTime));
        c_soundLibrary.PlayWaveClips();
        yield return specialPowerAnimTime;

        if (enemyTarget != null && enemyTarget.GetComponent<ClearDebrisTrigger>())
        {
            enemyTarget.GetComponent<ClearDebrisTrigger>().InitiateClear();
        }

        //reset animation, is attacking orb target, detroy te orb gameobject and reset DPAD
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
        isTargeted = false;
        Destroy(coldOrb, 1f);
        //ResetImageColor(celestPlayerDpad); //reset dpad colors
        isAttacking = false;


    }

    //Runs the animation flags for dodging
    private IEnumerator AnimateDodge()
    {
        celestialAnimator.animator.SetBool(celestialAnimator.IfDodgingHash, true);

        yield return dodgeAnimTime;
        
        celestialAnimator.animator.SetBool(celestialAnimator.IfDodgingHash, false);

    }

    //Handles the nuances of the physical movement as well as iframes (to be changed)
    private IEnumerator StopDodgeMovement()
    {
        StartIFrames();
        isDodging = true;
        this.GetComponent<CelestialPlayerMovement>().ToggleDodging(true);
        yield return dodgeMoveStopTime;
        EndIFrames();
        isDodging = false;
        this.GetComponent<CelestialPlayerMovement>().ToggleDodging(false);
        
    }


    /// <summary>
    /// HANDLES POWER DROPPING
    /// </summary>
    /// <param name="power"></param>
    /// <param name="position"></param>
    public void PowerDrop(PowerStats power, Vector3 position)
    {
        powerDrop = Instantiate(power.visualDisplay, position, Quaternion.identity);

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

    public Power GetEnemyWeakness(GameObject enemyTarget)
    {
        if (enemyTarget.GetComponent<Enemy>().enemyStats.enemyType == EnemyStats.enemyTypes.OilMonster)
        {
            return Power.COLDSNAP;
        }

        if (enemyTarget.GetComponent<Enemy>().enemyStats.enemyType == EnemyStats.enemyTypes.Smog)
        {
            return Power.MOONTIDE;
        }
        return Power.NONE;

    }


    /// <summary>
    /// HANDLES SENDING ENERGY CHANGES TO THE HUD
    /// </summary>
    /// <param name="pointsAdded"></param>
    public void IncreaseEnergy(int pointsAdded)
    {
        //if (CheckIfEnergyDrop(energy.current, -pointsAdded))
        energy.current += pointsAdded;
        OnPowerStateChange();
        //hudManager.IncreaseEnergy(pointsAdded);
        if (OnEnergyChanged != null)
            OnEnergyChanged(energy.current, energy.max);
    }

    public void DrainEnergy(int pointsDrained)
    {
        energy.current -= Mathf.Clamp(pointsDrained, 0, energy.max);
        if (CheckIfEnergyDrop(energy.current + pointsDrained, pointsDrained))
            OnPowerStateChange();
        if (OnEnergyChanged != null)
            OnEnergyChanged(energy.current, energy.max);
        //DecreaseEnergy(pointsDrained);
        //hudManager.DecreaseEnergy(pointsDrained);
    }

    public IEnumerator DrainRainEnergy()
    {
        int rainDrain = 1;


        // Gradually decrease fill amount over cooldown duration
        while (isRaining)
        {
            //Debug.Log("entering loop of coroutine");
            //float fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, timer / cooldownDuration);
            // fillImage.fillAmount = fillAmount;

            if (energy.current > 0)
            // Find the energy bar fill Image component dynamically
            {
                /*
                energy.current -= rainDrain;
                GameObject energyBar = GameObject.Find("EnergyBar"); // Assuming "energyBar" is the name of the GameObject holding the fill Image
                if (energyBar != null)
                {
                    Image fillImage = energyBar.transform.Find("Fill").GetComponent<Image>(); // Assuming "fill" is the name of the Image GameObject representing the fill
                    if (fillImage != null)
                    {
                        // Calculate the new fill amount
                        float fillAmount = fillImage.fillAmount - (float)rainDrain / 100f; // Assuming energy bar's max value is 100
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
                */
                //hudManager.DecreaseEnergy(rainDrain);
                DrainEnergy(rainDrain);
            }
            else
            {
                isRaining = false;
                buttonRain = false;
                RainParticleSystem.SetActive(false);

                NotEnoughEnergy(rainDrain, false);
            }

            yield return new WaitForSeconds(1);

        }
    }

    //Returns true if the current energy drain would make a power unusable
    private bool CheckIfEnergyDrop(int currentEnergy, int energyDrain)
    {
        if((currentEnergy - energyDrain < 1) && currentEnergy >= 1)
        {
            return true;
        }
        if((currentEnergy - energyDrain < powerBehaviour.ColdSnapStats.energyDrain) && currentEnergy >= powerBehaviour.ColdSnapStats.energyDrain)
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

    /*
    private void DecreaseEnergy( int pointsDrained)
    {
        // Find the energy bar fill Image component dynamically
        GameObject energyBar = GameObject.Find("EnergyBar"); // Assuming "energyBar" is the name of the GameObject holding the fill Image
        if (energyBar != null)
        {
            Image fillImage = energyBar.transform.Find("Fill").GetComponent<Image>(); // Assuming "fill" is the name of the Image GameObject representing the fill
            if (fillImage != null)
            {
                // Calculate the new fill amount
                float fillAmount = fillImage.fillAmount + (float)pointsDrained / 100f; // Assuming energy bar's max value is 100
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
    */


    /// <summary>
    /// ///////////////////////////////////can be deleted
    /// 
    /// </summary>
    /// <returns></returns>
    /*
    public IEnumerator ResetRain()
    {
        if (isRaining)
        {
            yield return new WaitForSeconds(10f);

            weatherState.skyState = WeatherState.SkyState.CLEAR;
            RainParticleSystem.SetActive(false);
            isRaining = false;

        }

    }
    */


    public void ShootTowardsTarget(GameObject Orb)
    {
        var step = 20 * Time.deltaTime; // calculate distance to move
        Orb.transform.position = Vector3.MoveTowards(Orb.transform.position, enemyTarget.transform.position, step);
        Orb.transform.LookAt(enemyTarget.transform.position, Vector3.up);

    }

    /*
    private void HandlePlayerDodge()
    {

    }
    */

    public void StartIFrames()
    {
        iFramesOn = true;
    }

    public void EndIFrames()
    {
        iFramesOn = false;
    }

    ///
    /// RESETS ABILITIES AFTER THEY ARE USED
    ///
    public void ResetColdSnap()
    {
        //StartCoroutine(ColdSnapCoolDownTime());
        powerInUse = Power.NONE;
        powerBehaviour.ColdSnapStats.isOnCooldown = true;
        float coldSnapTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats);

        StartCooldownUI("CastCold", coldSnapTimer);

        StartCoroutine(ResetCooldownTime(coldSnapTimer, powerBehaviour.ColdSnapStats));

        //keyboard UI
        /*
        if (coldSnapFill.gameObject.activeSelf)
        {
            StartCoroutine(CoolDownImageFill(coldSnapFill));
        }

        else if (!coldSnapFill.gameObject.activeSelf)
        {
            coldSnapFill.enabled = true;
            coldSnapFill.gameObject.SetActive(true);
            StartCoroutine(CoolDownImageFill(coldSnapFill));
        }

        // controller UI
        if (CTRLColdSnapFill.gameObject.activeSelf)
        {
            StartCoroutine(CoolDownImageFill(CTRLColdSnapFill)); 
        }
        else
        {
            CTRLColdSnapFill.enabled = true;
            CTRLColdSnapFill.gameObject.SetActive(true);
            StartCoroutine(CoolDownImageFill(CTRLColdSnapFill)); ;
        }
        */
        //hudManager.InitiateCooldownIndicator("CastCold", powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats));
    }

    /*
    public IEnumerator ColdSnapCoolDownTime()
    {
        //hudManager.InitiateCooldownIndicator("CastCold", powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats));
        if (OnCooldownStarted != null)
            OnCooldownStarted("CastCold", powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats));
        powerBehaviour.ColdSnapStats.isOnCooldown = true;
        powerInUse = Power.NONE;
        //buttonColdSnap = false;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats));
        powerBehaviour.ColdSnapStats.isOnCooldown = false;
        canColdSnap = true;
    }
    */


    public void ResetLightningStrike()
    {
        //StartCoroutine(LightningCoolDownTime());
        //StartCoroutine(CoolDownImageFill(lightingStrikeFill));
        //StartCoroutine(CoolDownImageFill(CTRLightingStrikeFill));
        powerInUse = Power.NONE;
        powerBehaviour.LightningStats.isOnCooldown = true;
        float lightningStrikeTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats);

        StartCooldownUI("CastThunder", lightningStrikeTimer);

        StartCoroutine(ResetCooldownTime(lightningStrikeTimer, powerBehaviour.LightningStats));
    }

    /*
    public IEnumerator LightningCoolDownTime()
    {
        //buttonLightningStrike = false;
        hudManager.InitiateCooldownIndicator("CastThunder", powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats));
        powerBehaviour.LightningStats.isOnCooldown = true;
        powerInUse = Power.NONE;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats));
        powerBehaviour.LightningStats.isOnCooldown = false;
        canLightningStrike = true;
    }
    */

    public void ResetBasic()
    {
        //StartCoroutine(BasicCoolDownTime());
        powerInUse = Power.NONE;
        powerBehaviour.BasicAttackStats.isOnCooldown = true;
        float basicAttackTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats);

        StartCooldownUI("BasicAttack", basicAttackTimer);

        StartCoroutine(ResetCooldownTime(basicAttackTimer, powerBehaviour.BasicAttackStats));
    }

    /*
    public IEnumerator BasicCoolDownTime()
    {
        buttonBasicAttack = false;
        hudManager.InitiateCooldownIndicator("BasicAttack", powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats));
        powerBehaviour.BasicAttackStats.isOnCooldown = true;
        powerInUse = Power.NONE;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats));
        powerBehaviour.BasicAttackStats.isOnCooldown = false;
        canBasicAttack = true;
    }
    */

    public void ResetMoonTide()
    {
        powerInUse = Power.NONE;
        powerBehaviour.MoonTideAttackStats.isOnCooldown = true;
        float moonTideTimer = powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats);

        StartCooldownUI("CastMoontide", moonTideTimer);

        StartCoroutine(ResetCooldownTime(moonTideTimer, powerBehaviour.MoonTideAttackStats));
        //StartCoroutine(MoonTideCoolDownTime());
        //moonTideFill.enabled = true;
        //CTRLMoonTideFill.enabled = true;
        //StartCoroutine(CoolDownImageFill(moonTideFill));
        //StartCoroutine(CoolDownImageFill(CTRLMoonTideFill));
        
    }

    /*
    public IEnumerator MoonTideCoolDownTime()
    {
        //buttonMoonTide = false;
        hudManager.InitiateCooldownIndicator("CastMoontide", powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats));
        
        
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats));
        powerBehaviour.MoonTideAttackStats.isOnCooldown = false;
        canMoonTide = true;
    }
    */


    public IEnumerator ResetCooldownTime(float timer, PowerStats powerStats)
    {
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
        //powerOnCooldown = false;
    }


    /// GETTING MOVED TO HUDMANAGER


    /*
    public IEnumerator CoolDownImageFill(Image fillImage)
    {

        float cooldownDuration = CoolDownTime(powerInUse);
        float timer = 0f;
        float startFillAmount = 1f;
        float endFillAmount = 0f;
        // Check if the fillImage is active, if not, activate it


        // Gradually decrease fill amount over cooldown duration
        while (timer < cooldownDuration)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, timer / cooldownDuration);
            fillImage.fillAmount = fillAmount;
            timer += Time.deltaTime;
            yield return null;

        }

        // Ensure fill amount is exactly 0
        fillImage.fillAmount = endFillAmount;
    }
    */

    /*
    public float CoolDownTime(Power powerinuse)
    {
        if (powerinuse == Power.MOONTIDE)
        {
            return powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats);

        }
        if (powerinuse == Power.COLDSNAP)
        {
            return powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats);

        }
        if (powerinuse == Power.LIGHTNINGSTRIKE)
        {
            return powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats);

        }
        if (powerinuse == Power.BASIC)
        {
            return powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats);

        }
        return 0f;
    }
    */

    /// <summary>
    /// HUD HELPER FUNCTIONS
    /// </summary>
    /// <returns></returns>


    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        celestialControls.controls.CelestialPlayerDefault.Disable();
        hudManager.SetCelesteOccupied(true);
        OnPowerStateChange();
        //celestialControls.controls.PlantIsSelected.Disable();
        //DarkenAllImages(uiManager.GetActiveUI()); //indicate no movement is allowed while planting
        //hudManager.ToggleCelestePanel(true);
        yield return waitTime;
        celestialControls.controls.CelestialPlayerDefault.Enable();
        //ResetImageColor(uiManager.GetActiveUI());
        hudManager.SetCelesteOccupied(false);
        OnPowerStateChange();
        //hudManager.ToggleCelestePanel(false);
    }

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange)
    {
        yield return waitTime;
    }

    //Darken UI icons
    /*
    void DarkenAllImages(GameObject targetGameObject)
    {
        if (targetGameObject != null)
        {
            Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                
                // Create a copy of the current material
                Material darkenedMaterial = new Material(image.material);

                // Darken the material color
                Color darkenedColor = darkenedMaterial.color * darkeningAmount;
                darkenedMaterial.color = darkenedColor;

                // Assign the new material to the image
                image.material = darkenedMaterial;
                
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {

        }
    }
        */


    // Function to reset color to original
    /*
    public void ResetImageColor(GameObject targetGameObject)
    {
        Image[] images = targetGameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            // Restore the original color
            image.color = Color.white;
        }
    }
    */

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

    public float GetEnergy()
    {
        return energy.current;
    }
}
