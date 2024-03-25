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
    [SerializeField] private GameObject celestPlayerDpad;
    [SerializeField] private float darkeningAmount = 0.5f; // how much to darken the images

    private NavMeshAgent celestialAgent;

    [Header("Rain System")]
    [SerializeField] WeatherState weatherState;
    [SerializeField] public bool isRaining = false;
    [SerializeField] public GameObject RainParticleSystem;


    [Header("Attack")]


    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool canBasicAttack = true;
    [SerializeField] public bool canColdSnap = true;
    [SerializeField] public bool canLightningStrike = true;
    [SerializeField] public bool canMoonTide = true;
    [SerializeField] public bool canSetFogTrap = true;
    [SerializeField] public bool canSetFrostTrap = true;
    [SerializeField] public bool canSunBeam = true;

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

    //Lengths of animations and abilities
   
    


    [Header("Animation")]
    private CelestialPlayerAnimator celestialAnimator;

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

    private void Awake()
    {
        OrigPos = new Vector3(20, 7, -97);
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
            interacting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            interacting = false;
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "Enemy")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = false;

            enemyLocation = other.transform.position;
            enemyTarget = null;

            //enemyTarget = null;



        }
    }

    // public void AttackEnemy()
    public void Attack()
    {

        PowerBehaviour attack;
        attack = GetComponent<PowerBehaviour>();

        bool playerIsDead;
        if (enemyTarget && powerInUse == Power.COLDSNAP)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.ColdSnapStats.maxDamage);
        }


        if (enemyTarget && powerInUse == Power.LIGHTNINGSTRIKE)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.LightningStats.minDamage);
        }


        if (enemyTarget && powerInUse == Power.BASIC)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.BasicAttackStats.minDamage);
        }



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
        else if (isRaining)
        {
            // yield return new WaitForSeconds(10f);

            weatherState.skyState = WeatherState.SkyState.CLEAR;

            isRaining = false;

        }




    }
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
    public void OnSnowFlakeSelected() {

        if (canColdSnap)
        {
            canColdSnap = false;
        }
        isAttacking = true;
        powerInUse = Power.COLDSNAP;
        Debug.Log("start");
        DarkenAllImages(celestPlayerDpad); //darken the dpad

    }

    public void OnBasicAttackSelected() {
    
        canBasicAttack= false;
        isAttacking = true;
        powerInUse = Power.BASIC;
        Debug.Log("startbasic");
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
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, true);
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
        yield return new WaitForSeconds(1.958f);
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
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
        celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);






        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null)
        {


        }


            isAttacking = true;
            yield return new WaitForSeconds(1.958f);
            celestialAnimator.animator.SetBool(celestialAnimator.IfCastingSpellHash, false);
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
        yield return new WaitForSeconds(1.208f);
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


        Debug.Log("basic reset");

        yield return new WaitForSeconds(powerBehaviour.BasicAttackStats.rechargeTimer);
       canBasicAttack = true;

    }

    protected override IEnumerator SuspendActions(WaitForSeconds waitTime)
    {
        celestialControls.controls.CelestialPlayerDefault.Disable();
        //celestialControls.controls.PlantIsSelected.Disable();
        DarkenAllImages(celestPlayerDpad); //indicate no movement is allowed while planting
        yield return waitTime;
        celestialControls.controls.CelestialPlayerDefault.Enable();
        ResetImageColor(celestPlayerDpad);
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
