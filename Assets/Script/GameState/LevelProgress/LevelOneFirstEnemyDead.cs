using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstEnemyDead : MonoBehaviour
{
    
    //Get your Level Manager object's levelOneEvents script
    [SerializeField] LevelOneEvents levelOneEvents;
    [SerializeField] LevelOneProgress levelOneProgress;
    GameObject treeSeedSpawn;
    public CelestialPlayer player;
    // Start is called before the first frame update
    void Start() { 

    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckIfDead();
    }

    public void CheckIfDead()
    {

        Vector3 pos = this.transform.position;
        //
        //Use some kind of metric to catch if the monster this is attached to is dead or dying
        //Make sure it's able to complete this function before you destroy the gameobject (the monster) it's attached to
        //
        if (player.enemyTarget.GetComponent<Enemy>().isDying)
        //Call this function
        {
            //Instantiate the tree seed here

            levelOneEvents.OnFirstMonsterDefeated();
            treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, pos, Quaternion.identity);
            //Instantiate the tree seed here

        }
    }
}
