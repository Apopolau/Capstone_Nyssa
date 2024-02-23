using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.AI;

public class CelestialPlayer : MonoBehaviour
{
    
    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private LayerMask tileMask;

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


    PowerBehaviour powerBehaviour;
    public CelestialPlayerControls celestialControls;


    [Header("Respawn")]
    public Stat health;
    public Vector3 OrigPos = new Vector3(20,7,-97);
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;




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




    private void Awake()
    {

        celestialAnimator = GetComponent<CelestialPlayerAnimator>();
        celestialAgent = GetComponent<NavMeshAgent>();
        celestialAgent.enabled = false;
        celestialControls = GetComponent<CelestialPlayerControls>();
        health = new Stat(100, 100, false);
        virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
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


 /*   private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered collision with " + other.gameObject.name);
        if ((other.gameObject.tag == "Enemy"))
        {
            //Debug.Log("Trigger enter");

            //Player is in range of enemy, in invading monster they can pursue the player


           // Debug.Log("I've collided with enemy");
                enemySeen = true;
          
           enemyLocation= other.transform.position;
            enemyTarget = other.transform.gameObject;

        }
       
    }*/

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





    public bool TakeHit()
    {

       health.current -= 10;
        
        Debug.Log(health.current);

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        bool isDead = health.current <= 0;
        if (isDead)
        {
           Respawn();
        }

        return isDead;

    }

    // public void AttackEnemy()
    public void Attack()
    {

        PowerBehaviour attack;
        attack = GetComponent<PowerBehaviour>();

        bool playerIsDead;
        if (enemyTarget)
        {
            playerIsDead = enemyTarget.GetComponent<Enemy>().TakeHit(attack.ColdSnapStats.maxDamage);
        }
        
     


    }

    private void Respawn()
    {

        health.current = 100;
        gameObject.transform.position = OrigPos;

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
            RainParticleSystem.SetActive(true);
            weatherState.skyState = WeatherState.SkyState.RAINY;
            isRaining = true;
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

    }

    public IEnumerator animateColdSnap()
    {
     
        Debug.Log("coldsnap is animated");
        GameObject coldOrb = powerBehaviour.GetComponent<PowerBehaviour>().ColdSnapStats.visualDisplay;


    
        GameObject clone= Instantiate(coldOrb,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);
        //clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * 10;


        //Attacking animation of player
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, true);
        celestialAnimator.animator.SetBool(celestialAnimator.IfWalkingHash, false);
       
       




        // Move our position a step closer to the target.
        var step = 5 * Time.deltaTime; // calculate distance to move

        if (enemyTarget != null)
        {
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, enemyTarget.transform.position, step);
        }
       //clone.GetComponent<Rigidbody>().velocity = Vector3.MoveTowards(clone.transform.position, enemyTarget.transform.position, step);

        isAttacking = true;
        yield return new WaitForSeconds(3f);
        celestialAnimator.animator.SetBool(celestialAnimator.IfAttackingHash, false);
        Destroy(clone);

    }

    public IEnumerator ResetColdSnap()
    {


        Debug.Log("coldsnaptimer reset");
      
        yield return new WaitForSeconds(powerBehaviour.ColdSnapStats.rechargeTimer);
        canColdSnap = true;

    }
    public IEnumerator ResetThundrerStrike()
    {
        yield return new WaitForSeconds(1f);
       
    }
}
