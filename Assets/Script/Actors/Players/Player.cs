using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Parent class of EarthPlayer and CelestialPlayer
public abstract class Player : MonoBehaviour
{
    [SerializeField] GameObject playerObj;

    [Header("Respawn")]
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;
    [SerializeField] public TextMeshProUGUI displayText;
    [SerializeField] public Image playerWarningBG;
    private WaitForSeconds waitTime = new WaitForSeconds(4.542f);

    protected bool isShielded;
    protected bool iFramesOn;
    protected bool isStaggered;
    protected bool isDead;

    public bool interacting = false;

    public Vector3 OrigPos;
    public Stat health;

    public event System.Action<int, int> OnHealthChanged;

    [Header("SFX")]
    [SerializeField] protected PlayerSoundLibrary soundLibrary;

    WaitForSeconds barrierLength = new WaitForSeconds(5);
    WaitForSeconds staggerLength = new WaitForSeconds(0.958f);
    WaitForSeconds deathAnimLength = new WaitForSeconds(1.458f);
    WaitForSeconds iFramesLength = new WaitForSeconds(0.5f);

    public bool TakeHit(int damageDealt)
    {
        if (!isShielded && !iFramesOn)
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
        isDying = true;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, true);
        CallSuspendActions(deathAnimLength);
        yield return deathAnimLength;
        isDying = false;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, false);
        Respawn();
    }

    protected IEnumerator ThrowPlayerWarning(string textInfo)
    {
        displayText.text = textInfo;
        // Check if the Image component is disabled
        if (!playerWarningBG.enabled)
        {
            playerWarningBG.enabled = true;
        }

        playerWarningBG.gameObject.SetActive(true);
        yield return waitTime;
        displayText.text = "";
        playerWarningBG.enabled = false;


    }

    private IEnumerator iFrames()
    {
        iFramesOn = true;
        yield return iFramesLength;
        iFramesOn = false;
    }

    public void ApplyBarrier()
    {
        isShielded = true;
        StartCoroutine(BarrierWearsOff());
    }

    private IEnumerator BarrierWearsOff()
    {
        yield return barrierLength;
        isShielded = false;
    }

    private void Respawn()
    {

        health.current = 100;
        gameObject.transform.position = OrigPos;
        isDead = false;
    }

    public int GetHealth()
    {
        return health.current;
    }

    public bool GetShielded()
    {
        return isShielded;
    }

    public GameObject GetGeo()
    {
        return playerObj;
    }

    public void SetHealth(int newHealthPoint)
    {
        health.current = newHealthPoint;

    }
    public void SetLocation(Vector3 newPosition)
    {
        gameObject.transform.position = newPosition;
    }

    public void CallSuspendActions(WaitForSeconds waitTime)
    {
        StartCoroutine(SuspendActions(waitTime));
    }

    protected abstract IEnumerator SuspendActions(WaitForSeconds waitTime);

    protected abstract IEnumerator SuspendActions(WaitForSeconds waitTime, bool boolToChange);

    public bool IsDead()
    {
        return isDead;
    }

    public bool IsStaggered()
    {
        return isStaggered;
    }
}
