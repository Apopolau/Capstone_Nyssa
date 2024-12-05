using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrigger : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    //[SerializeField] private float moveSpeed;
    private CelestialPlayer celestialPlayer;
    private List<Enemy> enemiesHit;
    //private Rigidbody rb;
    //private Vector3 moveDirection;
    //private bool initialized = false;

    private void Awake()
    {
        enemiesHit = new List<Enemy>();
        //rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if (initialized)
            //rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("Staff touched enemy");
            Enemy enemyBeingHit = other.gameObject.GetComponent<Enemy>();

            if (!enemyBeingHit.GetIsBeingHit())
            {
                enemyBeingHit.SetBeingHit(true);
                celestialPlayer.LightningAttack(enemyBeingHit);
                enemiesHit.Add(enemyBeingHit);
            }
        }

        if (other.GetComponent<ShutOffTerminal>())
        {
            other.GetComponent<ShutOffTerminal>().TerminalShutOff();
        }
    }

    /*
    public void InitializeSelf()
    {
        initialized = true;
    }
    */

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
        celestialPlayer.ClearLightning();
        Destroy(gameObject);
    }
}
