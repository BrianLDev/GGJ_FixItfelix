using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DemonSpawner : NightTimeListener
{
    [SerializeField] GameObject mindDemonPrefab, bodyDemonPrefab, soulDemonPrefab;
    private int mindDemons;
    private int bodyDemons;
    private int soulDemons;
    private static int demonsToSpawn;
    private List<GameObject> activeDemons;
    private float timeBetweenSpawns;
    private float timeToNextSpawn;
    [SerializeField] float timeToFirstSpawn = 1.0f;
    private bool isNightPhase = false;
    private float nightDuration;
    private 

    // Start is called before the first frame update
    void Start()
    {
        activeDemons = new List<GameObject>();
        Assert.IsNotNull(mindDemonPrefab);
        Assert.IsNotNull(bodyDemonPrefab);
        Assert.IsNotNull(soulDemonPrefab);
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
        demonsToSpawn = mindDemons + bodyDemons + soulDemons;
        timeBetweenSpawns = (cycle.GetNightDuration() - timeToFirstSpawn) / demonsToSpawn;
        timeToNextSpawn = timeToFirstSpawn;
    }

    public override void StartNewDay(DayNightCycle cycle) {
        Debug.Log("Demon spawner: A new day begins...");
        BeGoneDemons();
        isNightPhase = false;
        mindDemons = 0;
        bodyDemons = 0;
        soulDemons = 0;
    }

    private void spawningDemons() {
        timeToNextSpawn -= Time.fixedDeltaTime;

        if (timeToNextSpawn <= 0 && demonsToSpawn > 0) {
            // pick a random spot on the screen to spawn
            Vector3 spawnLocation = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0);
            spawnLocation = Camera.main.ScreenToWorldPoint(spawnLocation);

            Debug.Log("Spawning demon at: " + spawnLocation);
            // TODO: select mind, body, or soul demon
            GameObject demon = Instantiate(mindDemonPrefab, spawnLocation, Quaternion.identity);
            activeDemons.Add(demon);

            demonsToSpawn--;
            timeToNextSpawn = timeBetweenSpawns;
        }
    }

    private void BeGoneDemons() {
        if (activeDemons.Count > 0) {
            foreach (GameObject demon in activeDemons) {
                demon.GetComponent<DemonAI>().EndNightPhase();
            }
        }
    }

}
