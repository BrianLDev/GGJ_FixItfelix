using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : NightTimeListener
{
    [SerializeField] GameObject mindDemonPrefab, bodyDemonPrefab, soulDemonPrefab;
    private int mindDemons;
    private int bodyDemons;
    private int soulDemons;
    private float timeBetweenSpawns;
    private float timeToNextSpawn;

    private bool isNightPhase = false;
    private float nightDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        if (isNightPhase) {
            spawningDemons();
        }    
    }

    public override void StartNewNight(DayNightCycle cycle) {
        isNightPhase = true;
        mindDemons = cycle.GetNumMindDemons();
        bodyDemons = cycle.GetNumBodyDemons();
        soulDemons = cycle.GetNumSoulDemons();
        timeBetweenSpawns = cycle.GetNightDuration();
        timeToNextSpawn = timeBetweenSpawns;
    }

    public override void StartNewDay(DayNightCycle cycle) {
        isNightPhase = false;
        mindDemons = 0;
        bodyDemons = 0;
        soulDemons = 0;
    }

    private void spawningDemons() {
        // TODO: When day/night manager done, add SpawnDemons initiator
        timeToNextSpawn -= Time.fixedDeltaTime;
        if (timeToNextSpawn <= 0) {

        }
    }

}
