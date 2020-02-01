using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    
    GameObject currentTarget = null;
    public int speed = 10;

    // Start is called before the first frame update
    void Start()
    {
    }
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null) {
            FindTarget();
        }
        else {
            MoveTowardsBldg();
        }

    }

    public void SpawnDemon() {
        // TODO: update this later to initialize and spawn the demon
        m_DemonType = "Mind";  
    }

    public void FindTarget() {
        GameObject[] bldgList_Mind = GameObject.FindGameObjectsWithTag("Mind");

        // find building target
        foreach (GameObject bldgMind in bldgList_Mind) {
            if (bldgMind.tag == m_DemonType) {
                currentTarget = bldgMind;
                break;
            }
            else {
                Debug.Log("No target found...demon is lost");
            }
        }
    }

    public void MoveTowardsBldg() {
        Vector3 direction = (currentTarget.transform.position - transform.position);
        direction.Normalize();
        transform.Translate(direction*speed * Time.deltaTime);

        // Check the array to see if you find a building, saving the first, checking by proximity.
        // If you find a the m_DemonType tag on the building, thats the better target. If you see Vice as the tag, that's the best target
        // If the demon is next to a building, deal damage to it.    
    }

    public void EndNightPhase() {

    }
}
