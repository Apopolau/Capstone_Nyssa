using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Parent class of EarthPlayer and CelestialPlayer
public abstract class Player : MonoBehaviour
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

    protected bool isShielded;
    protected bool iFramesOn;
    protected bool isStaggered;
    protected bool isDead;

    protected bool interacting = false;

    protected Vector3 OrigPos;
    public Stat health;

    public event System.Action<int, int> OnHealthChanged;
    public event System.Action<string, float> OnCooldownStarted;

    [Header("SFX")]
    [SerializeField] protected PlayerSoundLibrary soundLibrary;

    [Header("Player Animation Stats")]
    [SerializeField] protected AnimationClip takeHitAnimation;
    [SerializeField] protected AnimationClip deathAnimation;

    protected float takeHitTime;
    protected float deathTime;

    protected WaitForSeconds staggerLength;
    protected WaitForSeconds deathAnimLength;


    WaitForSeconds barrierLength = new WaitForSeconds(5);
    WaitForSeconds iFramesLength = new WaitForSeconds(0.5f);

    public bool TakeHit(int damageDealt)
    {
        if(damageDealt > 0)
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

    private IEnumerator OnStagger()
    {
        isStaggered = true;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfTakingHitHash, true);
        CallSuspendActions(staggerLength);
        yield return staggerLength;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfTakingHitHash, false);
        isStaggered = false;
        StartCoroutine(iFrames());
    }

    private IEnumerator DeathRoutine()
    {
        //Set our bools to dying, stop players from doing anything while the animation plays out
        isDying = true;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, true);
        CallSuspendActions(deathAnimLength);

        yield return deathAnimLength;

        //Reset our bools, stop animation
        isDying = false;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, false);
        Respawn();
    }

    //Turns on and then off the player's invulnerability frames after a time
    private IEnumerator iFrames()
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

    //Suspend player actions for the specified time
    protected abstract IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange);
    
    protected void StartCooldownUI(string powerName, float timer)
    {
        if (OnCooldownStarted != null)
            OnCooldownStarted(powerName, timer);
        //yield return new WaitForSeconds(timer);
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
}
