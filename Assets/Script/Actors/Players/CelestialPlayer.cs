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

    //[SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;
    //[SerializeField] private GameObject celestPlayerDpad;
    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images


    [Header("Buttons Overlay")]
    [SerializeField] public Image coldSnapFill;
    [SerializeField] public Image lightingStrikeFill;
    [SerializeField] public Image moonTideFill;
    [SerializeField] public Image rainFill;

    [SerializeField] public Image CTRLColdSnapFill;
    [SerializeField] public Image CTRLightingStrikeFill;
    [SerializeField] public Image CTRLMoonTideFill;
    [SerializeField] public Image CTRLRainFill;
    private CelestUIManager uiManager;
    private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    [SerializeField] public bool isRaining = false;
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

    public Stat energy;
    PowerBehaviour powerBehaviour;
    public CelestialPlayerControls celestialControls;

    [Header("Power Drop Assets")]
    private VisualEffect powerDrop;
    GameObject coldOrb;
    GameObject moonTide;

    //Lengths of animations and abilities
    private WaitForSeconds dodgeAnimTime;
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
        energy = new Stat(100, 0, true);
        uiManager = GetComponent<CelestUIManager>();
        c_soundLibrary = base.soundLibrary as CelesteSoundLibrary;
        //virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }


    void Start()
    {

        powerBehaviour = GetComponent<PowerBehaviour>();
        basicPowerAnimTime = new WaitForSeconds(1.208f);
        specialPowerAnimTime = new WaitForSeconds(1.958f);
        //dodgeAnimTime = new WaitForSeconds();
        coldSnapCoolDownTime = powerBehaviour.ColdSnapStats.rechargeTimer;
        basicCoolDownTime = powerBehaviour.BasicAttackStats.rechargeTimer;
        lightningCoolDownTime = powerBehaviour.BasicAttackStats.rechargeTimer;
        staff = GetComponentInChildren<CelestialPlayerBasicAttackTrigger>();


        StartCoroutine(CoolDownImageFill(lightingStrikeFill));
        StartCoroutine(CoolDownImageFill(CTRLightingStrikeFill));

        rainFill.enabled = false;
        CTRLRainFill.enabled = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (isTargeted)
        {
            ShootTowardsTarget(coldOrb);
        }

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

    public void OnDodgeSelected(InputAction.CallbackContext context)
    {

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
        buttonColdSnap = true;
        powerInUse = Power.COLDSNAP;
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    public void OnBasicAttackSelected()
    {
        //BASICATTACK POWER
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        buttonBasicAttack = true; ;
        powerInUse = Power.BASIC;
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }

    public void OnLightningStrikeSelected()
    {
        //LIGHTNINGSTRIKE POWER 
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        buttonLightningStrike = true;
        powerInUse = Power.LIGHTNINGSTRIKE;
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
    }


    public void OnMoonTideSelected()
    {
        //MOONTIDE POWER
        //This specfic powers button was seleected, set Celeste current poweer to this specific power and darken the DPAD
        buttonMoonTide = true;
        powerInUse = Power.MOONTIDE;
        //DarkenAllImages(uiManager.GetActiveUI()); //darken the controls
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
        if (enemyTarget != null && staff.enemyHit)
        {
            BasicAttack();

        }


        StartCoroutine(SuspendActions(basicPowerAnimTime));
        c_soundLibrary.PlayAttackClips();
        yield return basicPowerAnimTime;
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
        if (enemyTarget && powerInUse == Power.COLDSNAP && canColdSnap == false)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }

    public void BasicAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.BASIC && canBasicAttack == false)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }

    public void LightningAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.LIGHTNINGSTRIKE && canLightningStrike == false)
        {
            Power weakness = GetEnemyWeakness(enemyTarget);
            int HitPoints = GetPowerHitDamage(weakness);
            enemyIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(HitPoints);
        }
    }

    public void MoonTideAttack()
    {
        bool enemyIsDead;
        if (enemyTarget && powerInUse == Power.MOONTIDE && canMoonTide == false)
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
    /// POWER SELECTION FUNCTIONALITY
    /// </summary>

    //if player selects raindrop
    public void OnRainDropSelected()
    {
        if (!isRaining)
        {
            // RainParticleSystem.SetActive(true);
            weatherState.SetRainyState(true);
            isRaining = true;
            rainFill.enabled = true;
            CTRLRainFill.enabled = true;
        }
        else if (isRaining)
        {
            // yield return new WaitForSeconds(10f);

            weatherState.SetRainyState(false);

            isRaining = false;
            rainFill.enabled = false;
            CTRLRainFill.enabled = false;

        }
        buttonRain = true;

    }

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
    



    ///
    /// RESETS ABILITIES AFTER THEY ARE USED
    ///
    public void ResetColdSnap()
    {
        StartCoroutine(ColdSnapCoolDownTime());

        //keyboard UI
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
    }

    public IEnumerator ColdSnapCoolDownTime()
    {
        buttonBasicAttack = false;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.ColdSnapStats));
        canColdSnap = true;
    }



    public void ResetLightningStrike()
    {
        StartCoroutine(LightningCoolDownTime());
        StartCoroutine(CoolDownImageFill(lightingStrikeFill));
        StartCoroutine(CoolDownImageFill(CTRLightingStrikeFill));
    }
    public IEnumerator LightningCoolDownTime()
    {
        buttonLightningStrike = false;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.LightningStats));
        canLightningStrike = true;
    }

    public void ResetBasic()
    {
        StartCoroutine(BasicCoolDownTime());
        powerInUse = Power.NONE;


    }
    public IEnumerator BasicCoolDownTime()
    {
        buttonBasicAttack = false;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.BasicAttackStats));
        canBasicAttack = true;
    }


    public void ResetMoonTide()
    {
        StartCoroutine(MoonTideCoolDownTime());
        moonTideFill.enabled = true;
        CTRLMoonTideFill.enabled = true;
        StartCoroutine(CoolDownImageFill(moonTideFill));
        StartCoroutine(CoolDownImageFill(CTRLMoonTideFill));
    }



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

    public IEnumerator MoonTideCoolDownTime()
    {
        buttonMoonTide = false;
        yield return new WaitForSeconds(powerBehaviour.getRechargeTimerFloat(powerBehaviour.MoonTideAttackStats));
        canMoonTide = true;
    }

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        celestialControls.controls.CelestialPlayerDefault.Disable();
        //celestialControls.controls.PlantIsSelected.Disable();
        DarkenAllImages(uiManager.GetActiveUI()); //indicate no movement is allowed while planting
        yield return waitTime;
        celestialControls.controls.CelestialPlayerDefault.Enable();
        ResetImageColor(uiManager.GetActiveUI());
    }

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange)
    {
        yield return waitTime;
    }

    //Darken UI icons
    void DarkenAllImages(GameObject targetGameObject)
    {
        if (targetGameObject != null)
        {
            Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                /*
                // Create a copy of the current material
                Material darkenedMaterial = new Material(image.material);

                // Darken the material color
                Color darkenedColor = darkenedMaterial.color * darkeningAmount;
                darkenedMaterial.color = darkenedColor;

                // Assign the new material to the image
                image.material = darkenedMaterial;
                */
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {

        }
    }

    // Function to reset color to original
    public void ResetImageColor(GameObject targetGameObject)
    {
        Image[] images = targetGameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            // Restore the original color
            image.color = Color.white;
        }
    }


}
