using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : NightTimeListener
{
    [SerializeField] GameObject mindDemonPrefab, bodyDemonPrefab, soulDemonPrefab;
    private int mindDemons;
    private int bodyDemons;
    private int soulDemons;
    private static int demonCount;
    private List<GameObject> activeDemons;
    private float timeBetweenSpawns;
    private float timeToNextSpawn;
    private bool isNightPhase = false;
    private float nightDuration;
    private 

    // Start is called before the first frame update
    void Start()
    {
        activeDemons = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
        if (isNightPhase) {
            spawningDemons();
            nightDuration -= Time.fixedDeltaTime;
        }    
    }

    public override void StartNewNight(DayNightCycle cycle) {
        Debug.Log("Demon spawner: enter night mode...");
        isNightPhase = true;
        mindDemons = cycle.GetNumMindDemons();
        bodyDemons = cycle.GetNumBodyDemons();
        soulDemons = cycle.GetNumSoulDemons();
        demonCount = mindDemons + bodyDemons + soulDemons;
        timeBetweenSpawns = cycle.GetNightDuration() / (demonCount + 1);
        timeToNextSpawn = timeBetweenSpawns;
    }

    public override void StartNewDay(DayNightCycle cycle) {
        Debug.Log("Demon spawner: A new day begins...");
        beGoneDemons();
        isNightPhase = false;
        mindDemons = 0;
        bodyDemons = 0;
        soulDemons = 0;
    }

    private void spawningDemons() {
        timeToNextSpawn -= Time.fixedDeltaTime;
        if (timeToNextSpawn <= 0 && demonCount > 0) {
            Vector3 spawnLocation = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0);
            spawnLocation = Camera.main.ScreenToWorldPoint(spawnLocation);

            Debug.Log("Spawning demon at: " + spawnLocation);
            // TODO: select mind, body, or soul demon
            GameObject demon = Instantiate(mindDemonPrefab, spawnLocation, Quaternion.identity);
            activeDemons.Add(demon);

            demonCount--;
            timeToNextSpawn = timeBetweenSpawns;
        }
    }

    private void beGoneDemons() {
        if (activeDemons.Count > 0) {
            foreach (GameObject demon in activeDemons) {
                demon.GetComponent<DemonAI>().EndNightPhase();
            }
        }
    }

}
