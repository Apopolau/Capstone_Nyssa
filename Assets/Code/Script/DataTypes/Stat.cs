using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int max;
    public int current;
    public bool low;

    public Stat(int maxStat, int currentStat, bool statState)
    {
        max = maxStat;
        current = currentStat;
        low = statState;
    }
}
