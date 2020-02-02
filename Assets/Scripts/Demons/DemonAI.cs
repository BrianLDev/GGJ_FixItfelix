using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public enum DemonState { off_screen, idle, targeting, moving, attacking, exiting }

    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    [SerializeField] int speedNormal = 8;
    [SerializeField] int speedExit = 16;
    [SerializeField] DemonState demonState = DemonState.off_screen;

    private GameObject[] bldgList;
    private int speed;
    private GameObject currentTarget = null;

    // Start is called before the first frame update
    void Start()
    {
    }
    private void Awake()
    {
        SpawnDemon();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the target still exists (other demons can destory the building before it gets there)
        if (currentTarget == null) {
            demonState = DemonState.targeting;
        }

        switch(demonState) {
            case DemonState.off_screen:
                // TODO: Add any code if needed here
                break;
            case DemonState.idle:
                // TODO: Add any code if needed here
                break;
            case DemonState.targeting:
                FindTarget();
                break;
            case DemonState.moving:
                // do nothing here.  handled in FixedUpdate
                break;
            case DemonState.attacking:
                Attacking();
                break;
            case DemonState.exiting:
                Exiting();
                break;
            default:
                break;
        }
    }

    void FixedUpdate() {
        if (demonState == DemonState.moving && currentTarget != null) {
            MoveTowardsTarget();
        }
    }

    public void SpawnDemon() {
        speed = speedNormal;
        m_DemonType = "Building";
        demonState = DemonState.targeting;
    }

    public void FindTarget() {
        bldgList = GameObject.FindGameObjectsWithTag("Building");

        // find building target
        foreach (GameObject bldg in bldgList) {
            if (bldg.tag == m_DemonType) {
                // target found, start moving towards it
                currentTarget = bldg;
                demonState = DemonState.moving;
                break;
            }
            else {
                Debug.Log("No target found...demon is lost");
            }
        }
    }

    public void MoveTowardsTarget() {
        Vector3 direction = (currentTarget.transform.position - transform.position);
        // check if reached target
        if (direction.magnitude <= .11) {
            // reached target.  Start attacking
            demonState = DemonState.attacking;
        }
        else {
            direction.Normalize();
            transform.Translate(direction*speed * Time.fixedDeltaTime); 
        }
    }

    private void Attacking() {
        //  TODO: Smash building
        currentTarget.GetComponentInChildren<BuildingHealth>().DealDamage(5);
        if (currentTarget.GetComponentInChildren<BuildingHealth>().CurrentHealth <= 0) {
            // building smashed!  need to retarget
            demonState = DemonState.targeting;
        }
    }

    public void EndNightPhase() {
        Debug.Log("DemonAI ending night phase...");
        demonState = DemonState.exiting;
        speed = speedExit;
        currentTarget = new GameObject();
        currentTarget.transform.position = new Vector3(999, 999, 0);
        Destroy(this, 5f);
    }

    private void Exiting() {
        Debug.Log("DemonAI exiting...");
        Vector3 direction = (currentTarget.transform.position - transform.position);
        direction.Normalize();
        transform.Translate(direction*speed * Time.fixedDeltaTime); 
    }
}
