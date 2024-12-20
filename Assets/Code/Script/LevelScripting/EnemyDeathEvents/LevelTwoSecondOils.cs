using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Level 2 Second Oils", fileName = "L2OilPairDeath")]
public class LevelTwoSecondOils : EnemyDeathBehaviour
{
    //Get your Level Manager object's levelEvents script
    [SerializeField] LevelManagerObject levelTwoManager;
    [SerializeField] LevelTwoEvents levelTwoEvents;

    public override void CheckIfDead(Enemy enemy)
    {
        levelTwoEvents = (LevelTwoEvents)levelTwoManager.eventManager;
        levelTwoEvents.CountDownArea3Monsters(enemy);
    }
}
