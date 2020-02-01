using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNightTimeListener : NightTimeListener
{
    public void Start()
    {
        print("Start up!");
    }
    public override void StartNewDay(DayNightCycle cycle)
    {
        print("Day! " + cycle.GetCurrentDay());
    }

    public override void StartNewNight(DayNightCycle cycle)
    {
        print("Night... " + cycle.GetNumMindDemons() + "m, " + cycle.GetNumBodyDemons() + "b, " + cycle.GetNumSoulDemons() + "s");
    }
}
