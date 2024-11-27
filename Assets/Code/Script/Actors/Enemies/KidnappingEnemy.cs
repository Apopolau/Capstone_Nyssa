using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KidnappingEnemy : Enemy
{
    [Header("Kidnapping Enemy variables")]
    //Drag and drop sets here
    [SerializeField] protected GameObjectRuntimeSet playerSet;
    [SerializeField] protected GameObjectRuntimeSet animalSet;

    [SerializeField] protected EnemyAttackTrigger attackTrigger;

    protected CelestialPlayer celestialPlayer;
    protected EarthPlayer earthPlayer;

    protected GameObject closestAnimal;
    protected GameObject closestPlayer;

    [SerializeField] protected GameObject kidnapAnimalTarget;
    protected Animal kidnappedAnimal;

    //Interaction with the player
    protected bool seesPlayer = false;
    protected bool inAttackRange = false;
    protected bool attackInitiated = false;
    protected Vector3 playerLocation;

    //Interaction with the animal
    protected bool seesAnimal = false;
    protected bool inKidnapRange = false;
    protected bool hasAnimal = false;
    protected bool isKidnapping = false;
    [SerializeField] protected List<Transform> escapeRoute = new List<Transform>();
    [SerializeField] protected Transform escapeWayPoint;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float kidnapRange;

    //Animation parameters
    protected WaitForSeconds attackTime;

    protected override void Awake()
    {
        base.Awake();
        attackTime = new WaitForSeconds(enemyStats.attackAnimTime);
        attackTrigger.SetEnemy(this);
    }

    private void Start()
    {
        foreach (GameObject g in playerSet.Items)
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
        
        if (isInvader)
        {
            StartCoroutine(CalculateAnimalDistance());

        }
    }

    protected override void OnDeath()
    {
        StartCoroutine(Die());
    }

    protected IEnumerator Die()
    {
        if (kidnappedAnimal != null)
            kidnappedAnimal.SetKidnapStatus(false);
        isDying = true;
        isStaggered = true;
        //eventManager.dyingEnemy = this;
        animator.SetAnimationFlag("die", true);
        yield return animator.GetAnimationWaitTime("die");
        if (isSpecial)
        {
            deathBehaviour.CheckIfDead(this);
        }
        Destroy(gameObject);
    }

    protected IEnumerator CalculatePlayerDistance()
    {
        while (true)
        {
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return attackTime;
            float distance = enemyStats.sightRange;
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
            {
                if (agent.hasPath)
                {
                    //enemyAnimator.animator.SetBool(enemyAnimator.IfWalkingHash, true);
                    animator.SetAnimationFlag("move", true);
                }
                else
                {
                    animator.SetAnimationFlag("move", false);
                }
            }
        }
    }

    protected IEnumerator CalculateAnimalDistance()
    {
        while (true)
        {
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return attackTime;
            float distance = enemyStats.sightRange;
            seesAnimal = false;

            foreach (GameObject animal in animalSet.Items)
            {
                if (Mathf.Abs((animal.transform.position - this.transform.position).magnitude) < distance)
                {
                    distance = Mathf.Abs((animal.transform.position - this.transform.position).magnitude);
                    SetClosestAnimal(animal);
                    if (!animal.GetComponent<Animal>().GetIsKidnapped())
                    { seesAnimal = true; }

                }
            }
            if (distance < kidnapRange)
            {
                inKidnapRange = true;
            }
            else
            {
                inKidnapRange = false;
            }
        }

    }

    //Turn on the attack collider so the monster can do damage
    public void AttackCollisionOn()
    {
        attackTrigger.ToggleAttacking(true);
    }

    //Turn off the attack collider so the monster won't do damage anymore
    public void AttackCollisionOff()
    {
        attackTrigger.ToggleAttacking(false);
    }



    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>

    public GameObjectRuntimeSet GetPlayerSet()
    {
        return playerSet;
    }

    public bool GetSeesPlayer()
    {
        return seesPlayer;
    }

    //Returns the closest player to this monster
    public GameObject GetClosestPlayer()
    {
        return closestPlayer;
    }

    //Sets a new closest player to the monster
    public void SetClosestPlayer(GameObject newPlayer)
    {
        closestPlayer = newPlayer;
    }


    /// ATTACK GETTERS AND SETTERS
    //Returns whether or not the monster is in the middle of an attack
    public bool GetAttackInitiated()
    {
        return attackInitiated;
    }

    public void SetAttackInitiated(bool initiated)
    {
        attackInitiated = initiated;
    }

    //Returns the range at which the monster should attempt to land an attack
    public float GetAttackRange()
    {
        return attackRange;
    }

    public bool GetInAttackRange()
    {
        return inAttackRange;
    }


    /// KIDNAP GETTERS AND SETTERS
    //Returns the animal marked as closest
    public GameObject GetClosestAnimal()
    {
        return closestAnimal;

    }

    //Sets a new animal marked as closest
    public void SetClosestAnimal(GameObject nextAnimal)
    {
        closestAnimal = nextAnimal;
    }

    //Returns the animal this monster is currently kidnapping
    public Animal GetKidnappedAnimal()
    {
        return kidnappedAnimal;
    }

    //Sets the animal a monster is trying to kidnap
    public void SetKidnappedAnimal(Animal animal)
    {
        kidnappedAnimal = animal;
    }

    //Get the marker the animal should move to when being kidnapped
    public GameObject GetKidnapAnimalTarget()
    {
        return kidnapAnimalTarget;
    }

    //Can the monster see an animal right now?
    public bool GetSeesAnimal()
    {
        return seesAnimal;
    }

    //Returns if the monster is in range to kidnap an animal
    public bool GetInKidnapRange()
    {
        return inKidnapRange;
    }

    //Returns if the monster is mid-kidnapping
    public bool GetIsKidnapping()
    {
        return isKidnapping;
    }

    //Returns the range at which the monster can kidnap an animal
    public float GetKidnapRange()
    {
        return kidnapRange;
    }


    /// WAYPOINT GETTERS AND SETTERS
    //Returns a list of waypoints to use as an escape route
    public List<Transform> GetEscapeRoute()
    {
        return escapeRoute;
    }

    public void SetEscapeRoute(List<Transform> route)
    {
        escapeRoute = route;
    }

    //Returns the specific location the monster should path to in order to escape with an animal
    public Transform GetEscapeWaypoint()
    {
        return escapeWayPoint;
    }

    //Set the location the monster should attempt to escape to while kidnapping
    public void SetEscapeWaypoint(Transform waypoint)
    {
        escapeWayPoint = waypoint;
    }
}
