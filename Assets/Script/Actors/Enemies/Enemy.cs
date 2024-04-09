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
    public GameObjectRuntimeSet plantSet;
    public GameObjectRuntimeSet animalSet;

    public CelestialPlayer celestialPlayer;
    public EarthPlayer earthPlayer;
    //public GameObject playerObj;
    public EnemyStats enemyStats;

    public EnemyInvadingPath invaderEnemyRoutes;
    
    public Stat health;
    [SerializeField] LevelEventManager eventManager;
    [SerializeField] public MonsterSoundLibrary soundLibrary;
   //////////////// [SerializeField] LevelOneEvents levelOneEvents;
    [SerializeField] public bool isDying =false;
    public bool isStaggered = false;
    public bool isColliding = false;

 
    private GameObject closestPlayer;
    private GameObject closestPlant;
    private GameObject closestAnimal;


    //Interaction with plants
    public bool seesPlant = false;
    public bool inSmotherRange = false;
    public bool smotherInitiated = false;
    public Vector3 plantLocation;

    //Interaction with the player
    public bool seesPlayer = false;
    public bool inAttackRange = false;
    public bool attackInitiated = false;
    public Vector3 playerLocation;
    Rigidbody rb;
    [SerializeField] float smotherRange;
    [SerializeField] float attackRange;
    [SerializeField] float kidnapRange;
    [SerializeField] float sightRange;

    //Interaction with the animal
    public bool seesAnimal = false;
    public bool inKidnapRange = false;
    public bool hasAnimal = false;
    public bool isKidnapping = false;
    public Transform escapeRoute = null;

    //InvaderCoder
    public bool isInvader;
    public bool isPathSelected = false;
    public List<Transform> chosenPath;

    private WaitForSeconds attackTime;
    private WaitForSeconds takeHitTime;
    private WaitForSeconds deathTime;

    public event System.Action<int, int> OnHealthChanged;

    void Awake()
    {
        attackTime = new WaitForSeconds(enemyStats.attackAnimTime);
        takeHitTime = new WaitForSeconds(enemyStats.takeHitAnimTime);
        deathTime = new WaitForSeconds(enemyStats.deathAnimTime);
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
        StartCoroutine(CalculatePlantDistance());
        if (isInvader)
        {
            StartCoroutine(CalculateAnimalDistance());

        }

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
            seesPlant = false;
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
            if (enemyStats.enemyType != EnemyStats.enemyTypes.OilMonster)
            { if(enemyMeshAgent.hasPath)
                {
                    enemyAnimator.animator.SetBool(enemyAnimator.IfWalkingHash, true);
                }
            
            }
   

        }
        
    }
    private IEnumerator CalculatePlantDistance()
    {
        while (true)
        {
            //Debug.Log("its true,calculating distance");
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return attackTime;
            float distance = sightRange;
            seesPlant = false;
            inSmotherRange = false;
            foreach (GameObject plant in plantSet.Items)
            {
                //Debug.Log("Going through the plants");
                if (Mathf.Abs((plant.GetComponent<Plant>().transform.position - this.transform.position).magnitude) < distance)
                {
                    distance = Mathf.Abs((plant.GetComponent<Plant>().transform.position - this.transform.position).magnitude);
                    closestPlant = plant;
                    //if its already smothered dont gang up
                    if (!plant.GetComponent<Plant>().isSmothered)
                    { seesPlant = true; }
                    //Debug.Log("Plastic Bag Sees a plant");
                }
            }
            if (distance < smotherRange)
            {
                //Debug.Log("in smother range");

                inSmotherRange = true;
            }
        }

    }
    private IEnumerator CalculateAnimalDistance()
    {
        while (true)
        {
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return attackTime;
            float distance = sightRange;
            seesAnimal = false;
            inKidnapRange = false;
            foreach (GameObject animal in animalSet.Items)
            {
                if (Mathf.Abs((animal.transform.position - this.transform.position).magnitude) < distance)
                {
                    distance = Mathf.Abs((animal.transform.position - this.transform.position).magnitude);
                    closestAnimal = animal;
                    if (!animal.GetComponent<Animal>().isKidnapped) 
                    { seesAnimal = true; }
                        
                }
            }
            if (distance < kidnapRange)
            {
                inKidnapRange = true;
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
    public void SmotherInitiated()
    {
        if (enemyStats.enemyType == EnemyStats.enemyTypes.PlasticBag)
        {
            StartCoroutine(SmotherPlant());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isColliding = true;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isColliding = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isColliding = false;

        }
    }
    public GameObject GetClosestPlant()
    {
        return closestPlant;
    
    }

    public void SetClosestPlant(GameObject nextAnimal)
    {
        closestPlant = nextAnimal;
    }

    public GameObject GetClosestAnimal()
    {
        return closestAnimal;

    }

    public void SetClosestAnimal(GameObject nextAnimal)
    {
        closestAnimal = nextAnimal;
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
    /// <summary>
    /// should be if smother or chokinghash
    /// </summary>
    /// <returns></returns>
    private IEnumerator SmotherPlant()
    {
        enemyAnimator.animator.SetBool(enemyAnimator.IfSmotheringHash, true);
        yield return takeHitTime;
        enemyAnimator.animator.SetBool(enemyAnimator.IfSmotheringHash, false);
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
