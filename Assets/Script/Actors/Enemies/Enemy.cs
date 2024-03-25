using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemyMeshAgent;
    public OilMonsterAnimator enemyAnimator;
    //Drag and drop player here
    public GameObjectRuntimeSet playerSet;
    public CelestialPlayer celestialPlayer;
    public EarthPlayer earthPlayer;
    //public GameObject playerObj;
    public EnemyStats enemyStats;
    
    
    public Stat health;
    [SerializeField] EventManager eventManager;
   //////////////// [SerializeField] LevelOneEvents levelOneEvents;
    [SerializeField] public bool isDying =false;
    public bool isStaggered = false;
    public bool isColliding = false;
    private GameObject closestPlayer;

    //Interaction with the player
    public bool seesPlayer = false;
    public bool inAttackRange = false;
    public bool attackInitiated = false;
    public Vector3 playerLocation;
    Rigidbody rb;

    [SerializeField] float attackRange;
    [SerializeField] float sightRange;

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
        //eventManager = GetComponent<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyMeshAgent = GetComponent<NavMeshAgent>();
        foreach(GameObject g in playerSet.Items)
        {
            if (g.GetComponent<EarthPlayer>())
            {
                earthPlayer = g.GetComponent<EarthPlayer>();
            }
            else if (g.GetComponent<CelestialPlayer>())
            {
                celestialPlayer = g.GetComponent<CelestialPlayer>();
            }
        }
        StartCoroutine(CalculatePlayerDistance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CalculatePlayerDistance()
    {
        while (true)
        {
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return attackTime;
            float distance = sightRange;
            seesPlayer = false;
            inAttackRange = false;
            foreach (GameObject go in playerSet.Items)
            {
                if (Mathf.Abs((go.GetComponent<Player>().GetGeo().transform.position - this.transform.position).magnitude) < distance)
                {
                    distance = Mathf.Abs((go.GetComponent<Player>().GetGeo().transform.position - this.transform.position).magnitude);
                    closestPlayer = go;
                    seesPlayer = true;
                }
            }
            if (distance < attackRange)
            {
                inAttackRange = true;
            }
        }
        
    }
    
    public bool TakeHit( int hitPoints)
    {
        
        health.current -= hitPoints;
        bool isDead = health.current <= 0;

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        if (isDead) {
            StartCoroutine(Die());
        }
        if (hitPoints > 0)
        {
            
            StartCoroutine(TakePlayerHit());
        }

        return isDead;
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isColliding = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isColliding = false;

        }
    }

    public GameObject GetClosestPlayer()
    {
        return closestPlayer;
    }

    public void SetClosestPlayer(GameObject newPlayer)
    {
        closestPlayer = newPlayer;
    }

    private IEnumerator TakePlayerHit()
    {
        enemyAnimator.animator.SetBool(enemyAnimator.IfTakingHitHash, true);
        isStaggered = true;
        yield return takeHitTime;
        enemyAnimator.animator.SetBool(enemyAnimator.IfTakingHitHash, false);
        isStaggered = false;
    }

    private IEnumerator Die()
    {
        isDying = true;
        isStaggered = true;
        eventManager.dyingEnemy = this;
        enemyAnimator.animator.SetBool(enemyAnimator.IfTakingHitHash, false);
        enemyAnimator.animator.SetBool(enemyAnimator.IfDyingHash, true);
        yield return deathTime;
        if (enemyStats.isSpecial)
        {
            enemyStats.deathBehaviour.CheckIfDead();
        }
        Destroy(gameObject);
    }
}
