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
        Debug.Log("Demon spawner: enter night mode!");
        isNightPhase = true;
        mindDemons = cycle.GetNumMindDemons();
        bodyDemons = cycle.GetNumBodyDemons();
        soulDemons = cycle.GetNumSoulDemons();
        timeBetweenSpawns = cycle.GetNightDuration() / (mindDemons + bodyDemons + soulDemons);
        timeToNextSpawn = timeBetweenSpawns;
    }

    public override void StartNewDay(DayNightCycle cycle) {
        Debug.Log("Demon spawner: night mode ending");
        isNightPhase = false;
        mindDemons = 0;
        bodyDemons = 0;
        soulDemons = 0;
    }

    private void spawningDemons() {
        // TODO: When day/night manager done, add SpawnDemons initiator
        // Debug.Log("TimeToNextSpawn - before subtract: " + timeToNextSpawn);
        timeToNextSpawn -= Time.fixedDeltaTime * 3;
        // Debug.Log("TimeToNextSpawn - after subtract: " + timeToNextSpawn);
        if (timeToNextSpawn <= 0) {
            Debug.Log("Spawning demon...");
            Instantiate(mindDemonPrefab, Vector3.zero, Quaternion.identity);
            timeToNextSpawn = timeBetweenSpawns;
        }
    }

}
