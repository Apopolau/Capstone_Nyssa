using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Parent class of EarthPlayer and CelestialPlayer
public abstract class Player : Actor
{
    [SerializeField] protected HUDManager hudManager;
    [SerializeField] protected UserSettingsManager userSettingsManager;

    [SerializeField] protected GameObject keyboardControls;
    [SerializeField] protected GameObject controllerControls;

    [SerializeField] GameObject playerObj;

    [Header("Respawn")]
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;
    //[SerializeField] public TextMeshProUGUI displayText;
    //[SerializeField] public Image playerWarningBG;
    private WaitForSeconds waitTime = new WaitForSeconds(4.542f);

    //protected bool isMoving;
    protected bool isShielded;
    protected bool iFramesOn;
    protected bool isStaggered;
    protected bool isDead;

    protected bool interacting = false;

    protected List<GameObject> validTargets;
    protected int validTargetIndex = 0;
    protected GameObject powerTarget;
    [SerializeField] protected float spellRange;
    protected float closestDistance;

    protected Vector3 OrigPos;
    public Stat health;

    public event System.Action<int, int> OnHealthChanged;
    public event System.Action<string, float> OnCooldownStarted;

    [Header("SFX")]
    [SerializeField] protected PlayerSoundLibrary soundLibrary;

    WaitForSeconds barrierLength = new WaitForSeconds(5);
    WaitForSeconds iFramesLength = new WaitForSeconds(0.5f);

    public bool TakeHit(int damageDealt)
    {
        if (damageDealt > 0)
        {
            if (!isShielded && !iFramesOn && !isDying && !isDead)
            {
                health.current -= damageDealt;

                if (OnHealthChanged != null)
                    OnHealthChanged(health.max, health.current);

                bool isDead = health.current <= 0;
                if (isDead)
                {
                    soundLibrary.PlayDeathClips();
                    StartCoroutine(DeathRoutine());
                }
                if (!isDead && !isStaggered)
                {
                    soundLibrary.PlayTakeHitClips();
                    StartCoroutine(OnStagger());
                }

                return isDead;
            }
            else if (isDying || isDead)
            {
                return isDead;
            }
        }
        else
        {
            health.current -= damageDealt;

            if (OnHealthChanged != null)
                OnHealthChanged(health.max, health.current);

            bool isDead = health.current <= 0;
            return isDead;
        }
        
        return false;
    }

    protected IEnumerator OnStagger()
    {
        isStaggered = true;
        SetStagger();
        yield return animator.GetAnimationWaitTime("hurt");
        isStaggered = false;
        StartCoroutine(iFrames());
    }

    protected void SetStagger()
    {
        animator.SetAnimationFlag("hurt", true);
        SuspendActions(true);
    }

    protected IEnumerator DeathRoutine()
    {
        //Set our bools to dying, stop players from doing anything while the animation plays out
        isDying = true;
        //GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, true);
        animator.SetAnimationFlag("die", true);
        CallSuspendActions(animator.GetAnimationWaitTime("die"));

        yield return animator.GetAnimationWaitTime("die");

        //Reset our bools, stop animation
        animator.SetAnimationFlag("die", false);
        isDying = false;
        //GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, false);
        Respawn();
    }

    //Turns on and then off the player's invulnerability frames after a time
    protected IEnumerator iFrames()
    {
        iFramesOn = true;
        yield return iFramesLength;
        iFramesOn = false;
    }

    //Starts the process of turning the thorn barrier on and off of the player
    public void ApplyBarrier()
    {
        isShielded = true;
        StartCoroutine(BarrierWearsOff());
    }

    //Turns the barrier off after it's worn off
    private IEnumerator BarrierWearsOff()
    {
        yield return barrierLength;
        isShielded = false;
    }

    //Restores the player to life at their spawn point
    protected void Respawn()
    {
        health.current = health.max;
        Debug.Log("Original position: " + OrigPos);
        gameObject.transform.position = OrigPos;
        isDead = false;

        //Update the health bar
        if (OnHealthChanged != null)
            OnHealthChanged(health.max, health.current);
    }

    

    //Start the process for turning off all player controls for a set amount of time
    public void CallSuspendActions(WaitForSeconds waitTime)
    {
        StartCoroutine(SuspendActions(waitTime));
    }

    //Suspend player actions for the specified time
    protected abstract IEnumerator SuspendActions(WaitForSeconds waitTime);

    public abstract void SuspendActions(bool suspend);
    
    protected void StartCooldownUI(string powerName, float timer)
    {
        if (OnCooldownStarted != null)
            OnCooldownStarted(powerName, timer);
        //yield return new WaitForSeconds(timer);
    }

    //Create a list of targets that are in range of your abilities
    protected bool JudgeDistance(Vector3 transform1, Vector3 transform2, float distance)
    {
        float calcDistance = Mathf.Abs((transform1 - transform2).magnitude);


        if (calcDistance <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Goes through the current list of targets in range, finds the closest one
    public void PickClosestTarget()
    {
        closestDistance = spellRange;
        int i = 0;
        foreach (GameObject potTarget in validTargets)
        {
            float distanceMeasured = Mathf.Abs((potTarget.transform.position - this.transform.position).magnitude);
            if (distanceMeasured < closestDistance)
            {
                validTargetIndex = i;
                closestDistance = distanceMeasured;
                powerTarget = potTarget;
            }
            i++;
        }
    }

    public GameObject GetPowerTarget()
    {
        return powerTarget;
    }

    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>

    //Returns the player's current health
    public int GetHealth()
    {
        return health.current;
    }

    //Returns whether or not the player is currently shielded
    public bool GetShielded()
    {
        return isShielded;
    }

    //Returns the player geometry object/visual
    public GameObject GetGeo()
    {
        return playerObj;
    }

    //Change the player's current health to a new value
    public void SetHealth(int newHealthPoint)
    {
        health.current = newHealthPoint;

    }

    //Moves the location of the player to a new location
    public void SetLocation(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    //Returns whether or not the player is dead
    public bool IsDead()
    {
        return isDead;
    }

    //Returns whether or not the player is staggered from an attack and unable to act
    public bool IsStaggered()
    {
        return isStaggered;
    }

    //Returns the player's reference to the level HUD Manager
    public HUDManager GetHudManager()
    {
        return hudManager;
    }

    //Returns the player UI element of keyboard movement controls
    public GameObject GetKeyboardMovementCtrls()
    {
        return keyboardControls;
    }

    //Returns the player UI element of controller movement controls
    public GameObject GetControllerMovementCtrls()
    {
        return controllerControls;
    }

    public bool GetIsInteracting()
    {
        return interacting;
    }

    public void SetInSoftLock(bool softLock)
    {
        animator.SetInSoftLock(softLock);
        if(softLock)
            SuspendActions(false);
    }
}
