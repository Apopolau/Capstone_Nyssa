using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/L1ThirdEnemy", fileName = "ThirdEnemyDeath")]
public class LevelOneThirdEnemyDead : EnemyDeathBehaviour
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
        levelOneEvents.OnThirdMonsterDefeated();

    }
}
