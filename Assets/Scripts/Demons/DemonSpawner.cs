using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class DemonSpawner : NightTimeListener
{
    #pragma warning disable 0649    // disable the warning for SerializeFields that are assigned within the Unity Editor
    [SerializeField] GameObject mindDemonPrefab, bodyDemonPrefab, soulDemonPrefab;
    [SerializeField] float timeToFirstSpawn = 1.0f;
    #pragma warning restore 0649    // restore the warning for SerializeFields that are assigned within the Unity Editor
    private int mindDemons;
    private int bodyDemons;
    private int soulDemons;
    private static int demonsToSpawn;
    private List<GameObject> activeDemons;
    private float timeBetweenSpawns;
    private float timeToNextSpawn;
    private bool isNightPhase = false;
    private float nightDuration;
    private 

    // Start is called before the first frame update
    void Start()
    {
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
        isNightPhase = true;
        activeDemons = new List<GameObject>();
        mindDemons = cycle.GetNumMindDemons();
        bodyDemons = cycle.GetNumBodyDemons();
        soulDemons = cycle.GetNumSoulDemons();
        demonsToSpawn = mindDemons + bodyDemons + soulDemons;
        timeBetweenSpawns = (cycle.GetNightDuration() - timeToFirstSpawn) / demonsToSpawn;
        timeToNextSpawn = timeToFirstSpawn;
    }

    public override void StartNewDay(DayNightCycle cycle) {
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

            GameObject demon = new GameObject();

            bool isDemonSpawned = false;
            while (!isDemonSpawned && demonsToSpawn > 0) {
                int selector = Random.Range(1, 4);  // 1-3. With ints, the max in Random.Range is exclusive

                if (selector == 1 && mindDemons > 0) {
                    demon = Instantiate(mindDemonPrefab, spawnLocation, Quaternion.identity);
                    mindDemons--;
                    isDemonSpawned = true;
                }
                else if (selector == 2 && bodyDemons > 0) {
                    demon = Instantiate(bodyDemonPrefab, spawnLocation, Quaternion.identity);
                    bodyDemons--;
                    isDemonSpawned = true;
                }
                else if (selector == 3 && soulDemons > 0) {
                    demon = Instantiate(soulDemonPrefab, spawnLocation, Quaternion.identity);
                    soulDemons--;
                    isDemonSpawned = true;
                }
            }

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

