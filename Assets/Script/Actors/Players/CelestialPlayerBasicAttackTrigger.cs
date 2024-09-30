using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialPlayerBasicAttackTrigger : MonoBehaviour
{
    [SerializeField] CapsuleCollider staffCollider;
    CelestialPlayer celestialPlayer;
    private List<Enemy> enemiesHit;
    public bool enemyHit = false;
    //GameObject currTarget;
    bool midAttack = false;

    private void Awake()
    {
        enemiesHit = new List<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("Staff touched enemy");
            Enemy enemyBeingHit = other.gameObject.GetComponent<Enemy>();
            if (midAttack)
            {
                if (!enemyBeingHit.beingHit)
                {
                    //Debug.Log("Staff struck enemy!");
                    PowerStats basicStats = celestialPlayer.GetComponent<PowerBehaviour>().BasicAttackStats;
                    bool enemyIsDead;
                    enemyBeingHit.beingHit = true;
                    int HitPoints = Random.Range(basicStats.minDamage, basicStats.maxDamage);
                    //if (currTarget)
                    //{ enemyIsDead = enemyBeingHit.TakeHit(HitPoints); }
                    enemyIsDead = enemyBeingHit.TakeHit(HitPoints);
                    enemiesHit.Add(enemyBeingHit);
                }

                //enemyHit = true;
                //currTarget = other.gameObject;
            }
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {

            other.GetComponent<Enemy>().beingHit = false;
            //enemyHit = true;
            //currTarget = other.gameObject;

        }
    }
    */


    public void BasicAttack(PowerStats basicStats)
    {
        bool enemyIsDead;

        //Power weakness = GetEnemyWeakness(enemyTarget);
        int HitPoints = Random.Range(basicStats.minDamage, basicStats.maxDamage);
        //Debug.Log("Hitpoints:" + HitPoints);
        //if (currTarget)
        //{ enemyIsDead = currTarget.GetComponent<Enemy>().TakeHit(HitPoints); }
        //midAttack = false;

    }

    public void ToggleAttacking(bool attacking)
    {
        //Debug.Log("In the middle of an attack: " + attacking);
        midAttack = attacking;
        if (midAttack)
        {
            staffCollider.enabled = true;
        }
        if (!midAttack)
        {
            staffCollider.enabled = false;
            foreach (Enemy enemy in enemiesHit)
            {
                enemy.beingHit = false;
            }
            enemiesHit.Clear();
        }
    }

    public void SetPlayer(CelestialPlayer celeste)
    {
        celestialPlayer = celeste;
    }

}
