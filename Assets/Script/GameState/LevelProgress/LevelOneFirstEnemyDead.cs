using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstEnemyDead : MonoBehaviour
{
    
    //Get your Level Manager object's levelOneEvents script
    [SerializeField] LevelOneEvents levelOneEvents;
    [SerializeField] LevelOneProgress levelOneProgress;
    GameObject treeSeedSpawn;

    public DialogueTrigger monsterDeadDialouge;
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
        pos.y += 4.0f; //added this because the position off the tree seed is kind off under ground
        //
        //Use some kind of metric to catch if the monster this is attached to is dead or dying
        //Make sure it's able to complete this function before you destroy the gameobject (the monster) it's attached to
        //
        if(player.enemyTarget != null)
        {
            if (player.enemyTarget.GetComponent<Enemy>().isDying)
            //Call this function
            {
                //Instantiate the tree seed here

                levelOneEvents.OnFirstMonsterDefeated();

                //start dialouge after monester dies
                monsterDeadDialouge.TriggerDialogue();
                treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, pos, Quaternion.identity);
                treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(pos.x + 1, pos.y + 1, pos.z), Quaternion.identity);
                treeSeedSpawn = Instantiate(levelOneProgress.treeSeedPrefab, new Vector3(pos.x - 1, pos.y - 1, pos.z), Quaternion.identity);
                //Instantiate the tree seed here

            }
        }
        
    }
}
