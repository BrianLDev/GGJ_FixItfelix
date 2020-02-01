using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NightTimeListener : MonoBehaviour
{
    public abstract void StartNewDay(DayNightCycle cycle);
    public abstract void StartNewNight(DayNightCycle cycle);
}
