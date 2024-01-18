using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneProgress : LevelProgress
{
    //Start of game condition, saved duck, destroyed oil spill
    bool hasSavedDuck;
    //Defeated the monster on the short path
    bool hasTallGrass;
    //Cleared the rock slide

    //Defeated the monster up the path
    bool cleanedLongPath;
    //Cleaned the water at the top of the path
    bool hasTurnedOffPump;

    int treeCount;
    int grassCount;
    int cattailCount;

    int treeGoal = 5;
    int grassGoal = 7;
    int cattailGoal = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
