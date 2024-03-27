using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : ScriptableObject
{
    protected bool skippable;

    public bool GetIsSkippable()
    {
        return skippable;
    }
}
