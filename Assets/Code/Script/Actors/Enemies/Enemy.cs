using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Actor
{
    [Header("Broad enemy variables")]
    [Header("Key variables")]
    [SerializeField] protected EnemyStats enemyStats;
    [SerializeField] protected bool isSpecial;
    [SerializeField] protected EnemyDeathBehaviour deathBehaviour;
    [SerializeField] protected bool isInvader;
    public Stat health;

    [Header("Scene references")]
    [SerializeField] protected EnemyInvadingPath invaderEnemyRoutes;
    [SerializeField] protected LevelEventManager eventManager;

    [Header("References to components")]
    [SerializeField] protected MonsterSoundLibrary soundLibrary;
    protected Rigidbody rb;

    [Header("Flags")]
    protected bool isDying = false;
    protected bool isStaggered = false;
    protected bool isColliding = false;
    protected bool beingHit = false;

    

    //Registered public events
    public event System.Action<int, int> OnHealthChanged;

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
        health = new Stat(enemyStats.maxHealth, enemyStats.maxHealth, false);
    }

    private void OnTriggerEnter(Collider other)
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

    public bool TakeHit( int hitPoints)
    {
        
        health.current -= hitPoints;
        bool isDead = health.current <= 0;

        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);

        if (isDead) {
            soundLibrary.PlayDeathClips();
            OnDeath();
        }
        if (hitPoints > 0)
        {
            soundLibrary.PlayTakeHitClips();
            StartCoroutine(TakePlayerHit());
        }

        return isDead;
       
    }
    
    
    private IEnumerator TakePlayerHit()
    {
        animator.SetAnimationFlag("hurt", true);
        isStaggered = true;
        yield return animator.GetAnimationWaitTime("hurt");
        animator.SetAnimationFlag("hurt", false);

        isStaggered = false;
    }


    protected abstract void OnDeath();

    protected abstract void InitializeAnimator();


    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>

    public EnemyStats GetEnemyStats()
    {
        return enemyStats;
    }

    public bool GetIsDying()
    {
        return isDying;
    }

    public bool GetIsStaggered()
    {
        return isStaggered;
    }

    public bool GetIsBeingHit()
    {
        return beingHit;
    }

    public void SetBeingHit(bool isHit)
    {
        beingHit = isHit;
    }


    public bool GetIsColliding()
    {
        return isColliding;
    }

    //Returns the animator. May need refactored
    public OurAnimator GetEnemyAnimator()
    {
        if(animator == null)
        {
            InitializeAnimator();
        }
        return animator;
    }

    public MonsterSoundLibrary GetMonsterSoundLibrary()
    {
        return soundLibrary;
    }
}
