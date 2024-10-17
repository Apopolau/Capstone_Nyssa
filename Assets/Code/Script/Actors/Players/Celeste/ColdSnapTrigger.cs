using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdSnapTrigger : MonoBehaviour
{
    [SerializeField] private CapsuleCollider coldOrbCollider;
    [SerializeField] private float moveSpeed;
    private CelestialPlayer celestialPlayer;
    private List<Enemy> enemiesHit;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool initialized = false;


    private void Awake()
    {
        enemiesHit = new List<Enemy>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(initialized)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("Staff touched enemy");
            Enemy enemyBeingHit = other.gameObject.GetComponent<Enemy>();

            if (!enemyBeingHit.GetIsBeingHit())
            {
                //Debug.Log("Staff struck enemy!");
                //PowerStats coldSnapStats = celestialPlayer.GetComponent<PowerBehaviour>().ColdSnapStats;
                //bool enemyIsDead;
                enemyBeingHit.SetBeingHit(true);
                //int HitPoints = Random.Range(coldSnapStats.minDamage, coldSnapStats.maxDamage);
                //if (currTarget)
                //{ enemyIsDead = enemyBeingHit.TakeHit(HitPoints); }
                //enemyIsDead = enemyBeingHit.TakeHit(HitPoints);
                celestialPlayer.ColdSnapAttack(enemyBeingHit);
                enemiesHit.Add(enemyBeingHit);
            }
        }
    }

    public void InitializeSelf(Vector3 direction)
    {
        moveDirection = direction;
        initialized = true;
    }

    public void SetPlayer(CelestialPlayer celeste)
    {
        celestialPlayer = celeste;
    }

    public void Die()
    {
        foreach (Enemy enemy in enemiesHit)
        {
            enemy.SetBeingHit(false);
        }
        enemiesHit.Clear();
        celestialPlayer.ClearColdSnap();
        Destroy(gameObject);
    }
}
