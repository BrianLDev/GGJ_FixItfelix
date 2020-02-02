using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public enum DemonState { off_screen, idle, targeting, moving, attacking, exiting }
    public enum DemonType { Mind, Body, Soul }
    [SerializeField] DemonType m_DemonType;
    [SerializeField] GameObject m_game_Manager;
    [SerializeField] DemonState demonState = DemonState.off_screen;
    [SerializeField] int speedNormal = 5;
    [SerializeField] int speedExit = 10;
    [SerializeField] float idleCountdown = 2f;
    [SerializeField] int damage = 10;
    [SerializeField] static private float timeToAttack = 0.5f;
    private float attackCountdown = 0.5f;

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
    }

    void FixedUpdate() {

        switch(demonState) {
            case DemonState.off_screen:
                // TODO: Add any code if needed here
                break;
            case DemonState.idle:
                DoIdle();
                break;
            case DemonState.targeting:
                FindTarget();
                break;
            case DemonState.moving:
                if (currentTarget != null) {
                    MoveTowardsTarget();
                }
                else {
                    demonState = DemonState.targeting;
                }
                break;
            case DemonState.attacking:
                DoAttack();
                break;
            case DemonState.exiting:
                BeGoneDemon();
                break;
            default:
                break;
        }
    }

    public void SpawnDemon() {
        speed = speedNormal;
        demonState = DemonState.idle;
    }

    public void DoIdle() {
        idleCountdown -= Time.fixedDeltaTime;
        if (idleCountdown <= 0) {
            demonState = DemonState.targeting;
        }
    }

    public void FindTarget() {
        bldgList = GameObject.FindGameObjectsWithTag("Building");

        // find building target
        foreach (GameObject bldg in bldgList) {
            Debug.Log(m_DemonType.ToString() + "Demon considering attacking: " + bldg.GetComponent<BuildingInfo>().BuildingType.ToString() );
            if (bldg.GetComponent<BuildingInfo>().BuildingType == BuildingType.Vice) {
                Debug.Log("ATTACKING!!");
                currentTarget = bldg;
                demonState = DemonState.moving;
                break;            
            }
            else if (bldg.GetComponent<BuildingInfo>().BuildingType.ToString() == m_DemonType.ToString() ) {
                Debug.Log("ATTACKING!!");
                currentTarget = bldg;
                demonState = DemonState.moving;
                break;
            }
            else {
                Debug.Log("No match found, attack whatever...\nATTACKING!!");
                currentTarget = bldgList[0];
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

    private void DoAttack() {
        attackCountdown -= Time.fixedDeltaTime;
        if (attackCountdown <= 0) {
            currentTarget.GetComponentInChildren<BuildingHealth>().DealDamage(damage);
            attackCountdown = timeToAttack;

            if (currentTarget.GetComponentInChildren<BuildingHealth>().CurrentHealth <= 0) {
                // building smashed!  Retarget to new building
                demonState = DemonState.targeting;
            }
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

    private void BeGoneDemon() {
        Debug.Log("DemonAI exiting...");
        Vector3 direction = (currentTarget.transform.position - transform.position);
        direction.Normalize();
        transform.Translate(direction*speed * Time.fixedDeltaTime); 
    }
}
