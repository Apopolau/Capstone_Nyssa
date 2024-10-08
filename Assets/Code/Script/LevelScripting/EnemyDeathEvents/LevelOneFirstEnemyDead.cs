using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Level 1 First Enemy", fileName = "FirstEnemyDeath")]
public class LevelOneFirstEnemyDead : EnemyDeathBehaviour
{

    //Get your Level Manager object's levelOneEvents script
    [SerializeField] LevelManagerObject levelOneManager;
    [SerializeField] LevelOneEvents levelOneEvents;
    //[SerializeField] LevelOneProgress levelOneProgress;


    //public CelestialPlayer player;
    // Start is called before the first frame update

    public override void CheckIfDead()
    {
        levelOneEvents = (LevelOneEvents)levelOneManager.eventManager;
        levelOneEvents.OnFirstMonsterDefeated();
        
    }
}
