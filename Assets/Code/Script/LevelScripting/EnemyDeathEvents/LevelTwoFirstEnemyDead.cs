using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Level 2 First Enemy", fileName = "L2FirstEnemyDeath")]
public class LevelTwoFirstEnemyDead : EnemyDeathBehaviour
{

    //Get your Level Manager object's levelOneEvents script
    [SerializeField] LevelManagerObject levelTwoManager;
    [SerializeField] LevelTwoEvents levelTwoEvents;
    //[SerializeField] LevelOneProgress levelOneProgress;


    //public CelestialPlayer player;
    // Start is called before the first frame update

    public override void CheckIfDead(Enemy enemy)
    {
        levelTwoEvents = (LevelTwoEvents)levelTwoManager.eventManager;
        levelTwoEvents.OnFirstMonsterDefeated();

    }
}
