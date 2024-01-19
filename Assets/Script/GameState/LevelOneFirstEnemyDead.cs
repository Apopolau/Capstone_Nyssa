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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        //
        //Use some kind of metric to catch if the monster this is attached to is dead or dying
        //Make sure it's able to complete this function before you destroy the gameobject (the monster) it's attached to
        //

        //Call this function
        levelOneEvents.OnFirstMonsterDefeated();

        //Instantiate the tree seed here
        treeSeedSpawn = Instantiate(treeSeedPrefab, this.transform);
    }
}
