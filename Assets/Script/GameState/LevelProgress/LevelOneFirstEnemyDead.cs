using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstEnemyDead : MonoBehaviour
{
    //Get your Level Manager object's levelOneEvents script
    [SerializeField] LevelOneEvents levelOneEvents;
    //Drag this from the plant prefabs folder
    [SerializeField] GameObject treeSeedPrefab;
    GameObject treeSeedSpawn;

    public Dialogue dialogueOnDefeat;
    public DialogueManager dialogueManager;
    public CelestialPlayer player;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckIfDead();
    }

    public void CheckIfDead()
    {
        
        
        //
        //Use some kind of metric to catch if the monster this is attached to is dead or dying
        //Make sure it's able to complete this function before you destroy the gameobject (the monster) it's attached to
        //
        if (player.enemyTarget.GetComponent<Enemy>().isDying)
        //Call this function
        {
            //treeSeedSpawn = Instantiate(treeSeedPrefab, this.transform);
            treeSeedSpawn = Instantiate(player.treeSeedPrefab, this.transform);
            levelOneEvents.OnFirstMonsterDefeated();

            //start secondDialouge
           // dialogueManager.StartDialogue(dialogueOnDefeat);
            //Instantiate the tree seed here
           
        }
    }
}
