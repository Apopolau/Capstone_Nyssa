using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Level 1 Final Oils", fileName = "L1OilPairDeath")]
public class LevelOneFifthMonsterDeath : EnemyDeathBehaviour
{
    //Get your Level Manager object's levelEvents script
    [SerializeField] LevelManagerObject levelOneManager;
    [SerializeField] LevelOneEvents levelOneEvents;

    public override void CheckIfDead(Enemy enemy)
    {
        levelOneEvents = (LevelOneEvents)levelOneManager.eventManager;
        levelOneEvents.CountDownFinalMonsters();
    }
}
