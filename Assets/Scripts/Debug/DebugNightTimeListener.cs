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
        Debug.Log("Day " + cycle.GetCurrentDay() );
    }

    public override void StartNewNight(DayNightCycle cycle)
    {
        Debug.Log("Night " + cycle.currentDay + ", duration: " + cycle.GetNightDuration() + " seconds.\n" + 
                  "Mind Demons: " + cycle.GetNumMindDemons() + ", Body Demons: " + cycle.GetNumBodyDemons() + ", Soul Demons: " + cycle.GetNumSoulDemons());
    }
}
