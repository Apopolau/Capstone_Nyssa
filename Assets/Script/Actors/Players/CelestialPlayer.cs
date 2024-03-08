using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;
using UnityEngine.VFX;
public class CelestialPlayer : MonoBehaviour
{
    
    //[SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;
    [SerializeField] private GameObject celestPlayerDpad;
    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    [SerializeField] public bool isRaining=false;
    [SerializeField] public GameObject RainParticleSystem;


    [Header("Attack")]


    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] public bool canBasicAttack = true;
    [SerializeField] public bool canColdSnap = true;
    [SerializeField] public bool canLightningStrike = true;
    [SerializeField] public bool canMoonTide = true;
    [SerializeField] public bool canSetFogTrap = true;
    [SerializeField] public bool canSetFrostTrap = true;
    [SerializeField] public bool canSunBeam = true;

    public bool interacting = false;

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


    [Header("Respawn")]
    public Stat health;
    public Vector3 OrigPos = new Vector3(20,7,-97);
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;
    public bool isShielded = false;

    //Lengths of animations and abilities
    WaitForSeconds barrierLength = new WaitForSeconds(5);
    WaitForSeconds staggerLength = new WaitForSeconds(0.958f);
    WaitForSeconds deathAnimLength = new WaitForSeconds(1.458f);
    WaitForSeconds iFramesLength = new WaitForSeconds(0.5f);


    [Header("Animation")]
    private CelestialPlayerAnimator celestialAnimator;




    [SerializeField] public GameObject treeSeedPrefab;
    //private CelestialPlayerInputActions celestialPlayerInput;
    private PlayerInput playerInput;
    // [Header("Lightning System")]
    // Start is called before the first frame update



    //Interaction with the player
    public bool enemySeen = false;
    public GameObject enemyTarget= null;
    public Vector3 enemyLocation;

    public event System.Action<int, int> OnHealthChanged;
    private bool iFramesOn;
    private bool isStaggered;


    private void Awake()
    {

        celestialAnimator = GetComponent<CelestialPlayerAnimator>();
        celestialAgent = GetComponent<NavMeshAgent>();
        celestialAgent.enabled = false;
        celestialControls = GetComponent<CelestialPlayerControls>();
        health = new Stat(100, 100, false);
        energy = new Stat(100, 0, true);
        //virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }


    void Start()
    {
        // celestialPlayerInput = GetComponent<CelestialPlayerInputActions>();
        //playerInput = GetComponent<PlayerInput>();
        powerBehaviour = GetComponent<PowerBehaviour>();
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Interacting");
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Not interacting anymore");
            interacting = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" )
        {

          //  Debug.Log("Trigger Stay");


            // Debug.Log("Entered collision with " + other.gameObject.name);
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = true;

            enemyLocation = other.transform.position;
            enemyTarget = other.transform.gameObject;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");

        /*Debug.Log("Trigger Exit" + other.transform.tag);

        Debug.Log("Trigger Exit-- - " + other.transform.name);*/

        if (other.transform.gameObject.tag == "Enemy")
        {
           /* Debug.Log("----Trigger Exit----" + other.transform.tag);

            Debug.Log("------Trigger Exit --- " + other.transform.name);*/


            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = false;

            enemyLocation = other.transform.position;
            enemyTarget = null;

            //enemyTarget = null;



        }
    }





    public bool TakeHit(int damageDealt)
    {
        if (!isShielded && !iFramesOn)
        {
            health.current -= damageDealt;

            //Debug.Log(health.current);

            if (OnHealthChanged != null)
                OnHealthChanged(health.max, health.current);

            bool isDead = health.current <= 0;
            if (isDead)
            {
                StartCoroutine(DeathRoutine());
            }
            if (!isDead && !isStaggered)
            {
                StartCoroutine(OnStagger());
            }

            return isDead;
        }
        return false;
    }

    private IEnumerator OnStagger()
    {
        celestialAnimator.animator.SetBool(celestialAnimator.IfTakingHitHash, true);
        SuspendActions(staggerLength);
        yield return staggerLength;
        celestialAnimator.animator.SetBool(celestialAnimator.IfTakingHitHash, false);
        isStaggered = false;
        StartCoroutine(iFrames());
    }

    private IEnumerator iFrames()
    {
        iFramesOn = true;
        yield return iFramesLength;
        iFramesOn = false;
    }

    private IEnumerator DeathRoutine()
    {
        celestialAnimator.animator.SetBool(celestialAnimator.IfDyingHash, true);
        CallSuspendActions(deathAnimLength);
        yield return deathAnimLength;
        celestialAnimator.animator.SetBool(celestialAnimator.IfDyingHash, false);
        Respawn();
    }

    public void ApplyBarrier()
    {
        isShielded = true;
        StartCoroutine(BarrierWearsOff());
    }

    private IEnumerator BarrierWearsOff()
    {
        yield return barrierLength;
        isShielded = false;
    }

    // public void AttackEnemy()
    public void Attack()
    {

        PowerBehaviour attack;
        attack = GetComponent<PowerBehaviour>();

        bool playerIsDead;
        if (enemyTarget && powerInUse==Power.COLDSNAP)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.ColdSnapStats.maxDamage);
        }


        if (enemyTarget && powerInUse == Power.LIGHTNINGSTRIKE)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.LightningStats.minDamage);
        }



    }

    private void Respawn()
    {

        health.current = 100;
        gameObject.transform.position = OrigPos;
        isDead = false;
    }
    


    public int GetHealth()
    {
        return health.current;
    }
    public void SetHealth(int newHealthPoint)
    {
        health.current = newHealthPoint;

    }
    public void SetLocation (Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    //if player selects raindrop
    public void OnRainDropSelected()
    {
        if (!isRaining)
        {
           // RainParticleSystem.SetActive(true);
            weatherState.skyState = WeatherState.SkyState.RAINY;
            isRaining = true;
        }

        //Debug.Log("It is currently raining");
        else if (isRaining)
        {
           // yield return new WaitForSeconds(10f);
            //Debug.Log("******It is no longer raining****");

            weatherState.skyState = WeatherState.SkyState.CLEAR;
    
            isRaining = false;

        }




    }
    public IEnumerator ResetRain()
    {
        //Debug.Log("It is currently raining");
        if (isRaining)
        {
            yield return new WaitForSeconds(10f);
            //Debug.Log("******It is no longer raining****");

            weatherState.skyState = WeatherState.SkyState.CLEAR;
            RainParticleSystem.SetActive(false);
            isRaining = false;

        }

    }
    public void OnSnowFlakeSelected() { 

        canColdSnap= false;
        isAttacking = true;
        powerInUse = Power.COLDSNAP;
        Debug.Log("start");
        DarkenAllImages(celestPlayerDpad); //darken the dpad

    }

    public void OnLightningStrikeSelected()
    {

        canLightningStrike = false;
        isAttacking = true;
        powerInUse = Power.LIGHTNINGSTRIKE;
        Debug.Log("startlightning");
        DarkenAllImages(celestPlayerDpad); //darken the dpad

    }

    public IEnumerator animateColdSnap()
    {
     
        Debug.Log("coldsnap is animated");
        ///   GameObject coldOrb = powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualDisplay;
        ///  GameObject clone= Instantiate(coldOrb,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);

       VisualEffect coldOrb = powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualDisplay;
        VisualEffect clone= Instantiate(coldOrb,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);



        //clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * 10;


        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);
       
       




        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null)
        {
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, enemyTarget.transform.position, step);
            //clone.
          // clone.SetVector3( enemyTarget.transform.position);
        }
       //clone.GetComponent<Rigidbody>().velocity = Vector3.MoveTowards(clone.transform.position, enemyTarget.transform.position, step);

        isAttacking = true;
        yield return new WaitForSeconds(3f);
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, false);
        Destroy(clone, 1f);
        ResetImageColor(celestPlayerDpad); //reset dpad colors

    }



    public IEnumerator animateLightningStrike()
    {

        Debug.Log("lighning is animated is animated");
       
        VisualEffect lightningStrike = powerBehaviour.GetComponent<PowerBehaviour>().LightningStats.visualDisplay;
        VisualEffect clone = Instantiate(lightningStrike, new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y + 20, enemyTarget.transform.position.z), Quaternion.identity);

        clone.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);


        //clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * 10;


        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);






        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null)
        {


        }


            isAttacking = true;
            yield return new WaitForSeconds(3f);
            celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, false);
            Destroy(clone, 1f);
            ResetImageColor(celestPlayerDpad); //reset dpad colors
        

    }


    public IEnumerator animateBasicAttack()
    {


       
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);






        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null)
        {


        }


        isAttacking = true;
        yield return new WaitForSeconds(3f);
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, false);
        ResetImageColor(celestPlayerDpad); //reset dpad colors


    }

    public IEnumerator ResetColdSnap()
    {


        Debug.Log("coldsnaptimer reset");
      
        yield return new WaitForSeconds(powerBehaviour.ColdSnapStats.rechargeTimer);
        canColdSnap = true;

    }
    public IEnumerator ResetLightningStrike()
    {
        yield return new WaitForSeconds(1f);
       
    }

    public IEnumerator ResetBasic()
    {


        Debug.Log("coldsnaptimer reset");

        yield return new WaitForSeconds(powerBehaviour.BasicAttackStats.rechargeTimer);
        canColdSnap = true;

    }



    public void CallSuspendActions(WaitForSeconds waitTime)
    {
        StartCoroutine(SuspendActions(waitTime));
    }

    private IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        celestialControls.controls.CelestialPlayerDefault.Disable();
        //celestialControls.controls.PlantIsSelected.Disable();
        DarkenAllImages(celestPlayerDpad); //indicate no movement is allowed while planting
        yield return waitTime;
        celestialControls.controls.CelestialPlayerDefault.Enable();
        ResetImageColor(celestPlayerDpad);
    }

    //Darken UI icons
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
            }
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
    
     // Function to reset color to original
    public void ResetImageColor(GameObject targetGameObject)
    {
        Image[] images = targetGameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                 // Restore the original color
                 image.material.color = image.color;
            }
    }
}
