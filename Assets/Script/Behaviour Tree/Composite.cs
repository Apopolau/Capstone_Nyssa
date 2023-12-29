using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class to Sequence and Selector
//Tells the branch of the tree how it should run
//Not implemented directly
public abstract class Composite : BTNode
{
    protected int CurrentChildIndex = 0;

    protected Composite(List<BTNode> childNodes) : base(childNodes)
    {
        children.AddRange(childNodes);
    }
}
