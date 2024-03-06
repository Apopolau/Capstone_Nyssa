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
    public bool isStaggered = false;

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
    private WaitForSeconds takeHitTime = new WaitForSeconds(1.5f);
    private WaitForSeconds deathTime = new WaitForSeconds(2.458f);

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
            

            StartCoroutine(Die());
        }
        if (hitPoints > 0)
        {
            enemyAnimator.animator.SetBool(enemyAnimator.IfTakingHitHash, true);
            StartCoroutine(TakePlayerHit());
        }

        return isDead;
       
    }
    
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

    private IEnumerator TakePlayerHit()
    {
        yield return takeHitTime;
        enemyAnimator.animator.SetBool(enemyAnimator.IfTakingHitHash, false);
    }

    private IEnumerator Die()
    {
        isStaggered = true;
        enemyAnimator.animator.SetBool(enemyAnimator.IfDyingHash, true);
        yield return deathTime;
        Destroy(gameObject);
    }
}
