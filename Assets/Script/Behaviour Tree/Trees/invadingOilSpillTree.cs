using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class invadingOilSpillTree : BTree
{
    protected override BTNode SetupTree()
    {
        //Remove this when you actually implement it
        throw new System.NotImplementedException();

        /*
         * Your behaviour tree will go in here: put your sequences after "new List<BTNode>"
        BTNode root = new Selector(new List<BtNode>
        {

        if they're in fog
        switch to wander behaviour

        if they have an animal
        escape with the animal back the way they came

        if they can see the player (collider)
        if close enough to player
        Attack player
        if they can see the player/not close enough
        Run at player

        if they can see an animal
        if close enough to animal
        Kidnap animal
        if not close enough
        Approach animal

        if they can see a plant/buildable
        if close enough
        attack plant/buildable (depending on enemy?)
        if not close enough
        approach plant/buildable

        if nothing else
        follow path towards animal nest, following waypoints

        });
        return root;
        */
    }
}
