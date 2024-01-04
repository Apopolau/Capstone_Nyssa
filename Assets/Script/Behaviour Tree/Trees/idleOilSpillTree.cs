using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class idleOilSpillTree : BTree
{
    protected override BTNode SetupTree()
    {
        //Remove this when you actually implement it
        throw new System.NotImplementedException();

        /*
         * Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BtNode>
        {

        if they can see the player (collider)
        if close enough to player
        Attack player
        if they can see the player/not close enough
        Run at player

        if nothing else
        switch to wander behaviour

        });
        return root;
        */
    }
}
