using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemyMeshAgent;
    public OilMonsterAnimator enemyAnimator;
   //Drag and drop player here
    public GameObject playerObj;
    public EnemyStats enemyStats;
    public CelestialPlayer player;
    public Stat health;
    [SerializeField] bool isFirst;
    [SerializeField] public bool isDying =false;

    //Interaction with the player
    public bool seesPlayer = false;
    public bool inAttackRange = false;
    public bool attackInitiated = false;
    public Vector3 playerLocation;
    Rigidbody rb;



    //Interaction with the animal
    public bool hasAnimal;
    public bool isKidnapping;



    private WaitForSeconds attackTime = new WaitForSeconds(1);

    public event System.Action<int, int> OnHealthChanged;

    void Awake()
    {
        health = new Stat(enemyStats.maxHealth, enemyStats.maxHealth, false);
        enemyAnimator = GetComponent<OilMonsterAnimator>();
    }


    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyMeshAgent = GetComponent<NavMeshAgent>();
        //player = GetComponent<CelestialPlayer>();

     



    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool TakeHit( int hitPoints)
    {


        health.current -= hitPoints;
        bool isDead = health.current <= 0;

        Debug.Log("Enemy:" + health.current);

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        if (isDead) {

            Debug.Log("DIEEEEEEEE" );
            ////change scene herrrre
            ///
            if (player.enemyTarget.GetComponent<Enemy>().isFirst)
            {
                player.enemyTarget.GetComponent<Enemy>().isDying = true;
                player.enemyTarget.GetComponent<LevelOneFirstEnemyDead>().CheckIfDead();


            }
            else if (!player.enemyTarget.GetComponent<Enemy>().isFirst)
            {
                player.enemyTarget.GetComponent<Enemy>().isDying = true;
                player.enemyTarget.GetComponent<LevelOneGrassEnemyDead>().CheckIfDead();


            }
            

            Die();
        }

        return isDead;
       
    }
    /*private void OnTriggerEnter(Collider other)
    {

       // Debug.Log("Entered collision with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
            //Debug.Log("~~~~~~~~~~~~~~~~~~~Entered collision with " + other.gameObject.name); 
            playerLocation = other.transform.position;
     
        }
       /* if (other.gameObject.tag == "plant")
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
           // Debug.Log("~~~~~~~~~~~~~~~~~~~Entered collision with " + other.gameObject.name);
            playerLocation = other.transform.position;

        }


    }*/
        private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Coliding with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = true;
            playerLocation = other.transform.position;
            //Debug.Log("~~~~~~~~~~~~~~~~~~~Coliding with " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Exited collision with " + other.gameObject.name);
        if (other.gameObject == playerObj)
        {
            //Player is in range of enemy, in invading monster they can pursue the player
            seesPlayer = false;
            playerLocation = other.transform.position;





            //Debug.Log("~~~~~~~~~~~~~~~~~Exited collision with " + other.gameObject.name);
        }
    }

    private void Die()
    {
        
        Destroy(gameObject);
    }
}
