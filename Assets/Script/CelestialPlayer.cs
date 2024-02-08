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

    public Power powerInUse = Power.NONE;


    ColdSnapBehaviour coldSnap;



    [Header("Respawn")]
    private int healthPoints;
    public Vector3 OrigPos = new Vector3(20,7,-97);
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;
   
    
    
    
    
   
    [SerializeField] public GameObject treeSeedPrefab;
    //private CelestialPlayerInputActions celestialPlayerInput;
    private PlayerInput playerInput;
    // [Header("Lightning System")]
    // Start is called before the first frame update



    //Interaction with the player
    public bool enemySeen = false;

    public GameObject enemyTarget;
    public Vector3 enemyLocation;






    private void Awake()
    {
        celestialAgent = GetComponent<NavMeshAgent>();
        celestialAgent.enabled = false;
        healthPoints=100;
       virtualMouseInput.gameObject.GetComponentInChildren<Image>().enabled = false;
    }


    void Start()
    {
        // celestialPlayerInput = GetComponent<CelestialPlayerInputActions>();
        //playerInput = GetComponent<PlayerInput>();
        coldSnap = GetComponent<ColdSnapBehaviour>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered collision with " + other.gameObject.name);
        if (other.gameObject.tag == "enemy")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
           enemySeen = true;
          
           enemyLocation= other.transform.position;
            enemyTarget = other.gameObject;

        }
       
    }

    private void OnTriggerStay(Collider other)
    {   
        if (other.gameObject.tag == "Enemy")
        {
           // Debug.Log("Entered collision with " + other.gameObject.name);
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = true;

            enemyLocation = other.transform.position;
            enemyTarget = other.gameObject;

        }
    }
    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Entered collision with " + other.gameObject.name);
            //Player is in range of enemy, in invading monster they can pursue the player
            enemySeen = false;

            enemyLocation = other.transform.position;
          

        }
    }





    public bool TakeHit()
    {

       healthPoints -= 10;
        
        Debug.Log(healthPoints);
        bool isDead = healthPoints <= 0;
        if (isDead)
        {
           Respawn();
        }

        return isDead;

    }

    public void Attack()
    {

        ColdSnapBehaviour attack;
        attack = GetComponent<ColdSnapBehaviour>();

        bool playerIsDead;
        playerIsDead =enemyTarget.GetComponent<Enemy>().TakeHit(attack.ColdSnapStats.maxDamage);
        if (playerIsDead)
        {
            //player.enemyTarget.GetComponent<Enemy>();


        }


    }

    private void Respawn()
    {

        healthPoints = 100;
        gameObject.transform.position = OrigPos;

    }
    


    public int GetHealth()
    {
        return healthPoints;
    }
    public void SetHealth(int newHealthPoint)
    {
        healthPoints = newHealthPoint;

    }
    public void SetLocation (Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    //if player selects raindrop
    public void OnRainDropSelected() {
        //RainParticleSystem.SetActive(true);
        isRaining = true;

    }
    public IEnumerator ResetRain()
    {
        Debug.Log("It is currently raining");
        //if (isRaining)
       // {
            yield return new WaitForSeconds(5f);
            Debug.Log("******It is no longer raining****");
        isRaining = true;

        // RainParticleSystem.SetActive(false);


        //}

    }
 
    public void OnSnowFlakeSelected() { 
    
    }

    public IEnumerator animateColdSnap()
    {
     
        Debug.Log("coldsnap is animated");
        GameObject coldOrb = coldSnap.GetComponent<ColdSnapBehaviour>().ColdSnapStats.visualDisplay;
        GameObject clone= Instantiate(coldOrb,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z), Quaternion.identity);
        clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * 10;
      
        isAttacking = true;
        yield return new WaitForSeconds(2f);
       
        Destroy(clone);

    }

    public IEnumerator ResetColdSnap()
    {


        Debug.Log("coldsnaptimer reset");
      
        yield return new WaitForSeconds(coldSnap.ColdSnapStats.rechargeTimer);
        canColdSnap = true;

    }
    public IEnumerator ResetThundrerStrike()
    {
        yield return new WaitForSeconds(1f);
       
    }
}
