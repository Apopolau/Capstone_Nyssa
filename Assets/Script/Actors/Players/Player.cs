using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class of EarthPlayer and CelestialPlayer
public abstract class Player : MonoBehaviour
{
    [SerializeField] GameObject playerObj;

    [Header("Respawn")]
    [SerializeField] public bool isDying = false;
    [SerializeField] public bool isRespawning = false;

    protected bool isShielded;
    protected bool iFramesOn;
    protected bool isStaggered;
    protected bool isDead;

    public bool interacting = false;

    public Vector3 OrigPos;
    public Stat health;

    public event System.Action<int, int> OnHealthChanged;

    WaitForSeconds staggerLength = new WaitForSeconds(0.958f);
    WaitForSeconds deathAnimLength = new WaitForSeconds(1.458f);
    WaitForSeconds iFramesLength = new WaitForSeconds(0.5f);

    public bool TakeHit(int damageDealt)
    {
        if (!isShielded && !iFramesOn)
        {
            health.current -= damageDealt;

            //Debug.Log(health.current);

            if (OnHealthChanged != null)
                OnHealthChanged(health.max, health.current);

            bool isDead = health.current <= 0;
            if (isDead)
            {
                StartCoroutine(DeathRoutine());
            }
            if (!isDead && !isStaggered)
            {
                StartCoroutine(OnStagger());
            }

            return isDead;
        }
        return false;
    }

    private IEnumerator OnStagger()
    {
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfTakingHitHash, true);
        CallSuspendActions(staggerLength);
        yield return staggerLength;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfTakingHitHash, false);
        isStaggered = false;
        StartCoroutine(iFrames());
    }

    private IEnumerator DeathRoutine()
    {
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, true);
        CallSuspendActions(deathAnimLength);
        yield return deathAnimLength;
        GetComponent<PlayerAnimator>().animator.SetBool(GetComponent<PlayerAnimator>().IfDyingHash, false);
        Respawn();
    }

    private IEnumerator iFrames()
    {
        iFramesOn = true;
        yield return iFramesLength;
        iFramesOn = false;
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

    public bool IsDead()
    {
        return isDead;
    }
}
