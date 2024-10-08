using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyDeathBehaviour : ScriptableObject
{
    public CelestialPlayer celestialPlayer;

    public abstract void CheckIfDead();
}
