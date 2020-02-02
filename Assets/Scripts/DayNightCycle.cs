using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public int[] numberOfMindDemons;
    public int[] numberOfBodyDemons;
    public int[] numberOfSoulDemons;

    public GameObject levelManager;

    public float nightDuration = 10.0f;
    private float nightTimeLeft = 0.0f;

    private int currentDay = 0;

    private int randomMindDemons = 0;
    private int randomBodyDemons = 0;
    private int randomSoulDemons = 0;

    private LevelStateScript lss;

    // Start is called before the first frame update
    void Start()
    {
        lss = levelManager.GetComponent<LevelStateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nightTimeLeft > 0.0f)
        {
            nightTimeLeft -= Time.deltaTime;
            if (nightTimeLeft < 0.0f)
            {
                StartNewDay();
            }
        }
    }

    public bool IsNightTime()
    {
        return nightTimeLeft > 0.0f;
    }

    public float GetNightDuration()
    {
        return nightTimeLeft;
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    public int GetNumMindDemons()
    {
        int day = GetCurrentDay();
        if (day < numberOfMindDemons.Length)
        {
            return numberOfMindDemons[day];
        }
        else
        {
            return randomMindDemons;
        }
    }

    public int GetNumBodyDemons()
    {
        int day = GetCurrentDay();
        if (day < numberOfBodyDemons.Length)
        {
            return numberOfBodyDemons[day];
        }
        else
        {
            return randomBodyDemons;
        }
    }

    public int GetNumSoulDemons()
    {
        int day = GetCurrentDay();
        if (day < numberOfSoulDemons.Length)
        {
            return numberOfSoulDemons[day];
        }
        else
        {
            return randomSoulDemons;
        }
    }

    void RandomizeDemonSpawns()
    {
        int total = Mathf.CeilToInt(Mathf.Pow(1.5f, currentDay)) - 3;
        if (total < 0)
        {
            total = 0;
        }

        randomMindDemons = 1 + Random.Range(0, total);
        total -= randomMindDemons;
        randomBodyDemons = 1 + Random.Range(0, total);
        total -= randomBodyDemons;
        randomSoulDemons = 1 + total;
    }

    public void StartNewDay()
    {
        currentDay = currentDay + 1;
        nightTimeLeft = 0.0f;

        lss.UpdatePlayerMind(currentDay * 100.0f);

        if (currentDay >= numberOfMindDemons.Length)
        {
            RandomizeDemonSpawns();
        }

        NightTimeListener[] spawners = FindObjectsOfType<NightTimeListener>();
        foreach (NightTimeListener listener in spawners)
        {
            listener.StartNewDay(this);
        }
    }

    public void StartNewNight(float howLong = 0.0f)
    {
        if (nightTimeLeft != 0.0f)
        {
            // Don't start a new night while there's one active!
            Debug.Assert(true);
            return;
        }

        if (howLong == 0.0f)
        {
            howLong = nightDuration;
        }

        nightTimeLeft = howLong;

        NightTimeListener[] spawners = FindObjectsOfType<NightTimeListener>();
        foreach (NightTimeListener listener in spawners)
        {
            listener.StartNewNight(this);
        }
    }
}
