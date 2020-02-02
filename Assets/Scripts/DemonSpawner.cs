using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    public float x_min, x_max, y_min, y_max;
    public List<GameObject> demonObjects;

    private bool isNightPhase = false;
    private float nightDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNightPhase) {
            spawningDemons();
        }
    }

    public void beginNightPhase() {
        isNightPhase = true;
    }

    private void spawningDemons() {
        // TODO: When day/night manager done, add SpawnDemons initiator
    }

    public void endNightPhase() {
        isNightPhase = false;
    }
}
