using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialPlayerBasicAttackTrigger : MonoBehaviour
{
    public bool enemyHit = false;
    GameObject currTarget;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {



            enemyHit = true;
            currTarget = other.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.transform.gameObject.tag == "Enemy")
        {

            enemyHit = false;

            //enemyTarget = null;



        }
    }


    public void BasicAttack(PowerStats basicStats)
    {
        bool enemyIsDead;

        //Power weakness = GetEnemyWeakness(enemyTarget);
        int HitPoints = Random.Range(basicStats.minDamage, basicStats.maxDamage);
        //Debug.Log("Hitpoints:" + HitPoints);
        if (currTarget)
        { enemyIsDead = currTarget.GetComponent<Enemy>().TakeHit(HitPoints); }

    }





}
