using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public enum DemonState { off_screen, idle, targeting, moving, attacking, vice, exiting }

    [SerializeField] string m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    [SerializeField] int speed = 10;
    [SerializeField] DemonState demonState = DemonState.off_screen;

    private GameObject[] bldgList;

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
            case DemonState.vice:
                Vice();
                break;
            case DemonState.exiting:
                // TODO: Add any code if needed here
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
        // TODO: update this later to initialize and spawn the demon
        m_DemonType = "Building";
        demonState = DemonState.targeting;
    }

    public void FindTarget() {
        bldgList = GameObject.FindGameObjectsWithTag("Building");

        // find building target
        foreach (GameObject bldg in bldgList) {
            if (bldg.tag == m_DemonType) {
                // target found, start moving towards it
                Debug.Log("Target found: " + bldg);
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
        Debug.Log("Magnitude = " + direction.magnitude);
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

    public void Attacking() {
        //  TODO: Smash building
        currentTarget.GetComponentInChildren<BuildingHealth>().DealDamage(5);
        if (currentTarget.GetComponentInChildren<BuildingHealth>().CurrentHealth <= 0) {
            // building smashed!  need to retarget
            demonState = DemonState.targeting;
        }
    }

    public void Vice() {
        //  TODO: All of the vices
    }

    public void EndNightPhase() {
        // TODO: add code here for demon to leave the scene
    }
}
