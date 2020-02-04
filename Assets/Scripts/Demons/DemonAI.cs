using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649    // disable the warning for SerializeFields that are assigned within the Unity Editor

public class DemonAI : MonoBehaviour
{
    #pragma warning disable 0649    // disable the warning for SerializeFields that are assigned within the Unity Editor
    public enum DemonState { off_screen, idle, targeting, moving, attacking, exiting }
    public enum DemonType { Mind, Body, Soul }
    [SerializeField] DemonType demonType;
    [SerializeField] GameObject m_game_Manager;
    [SerializeField] DemonState demonState = DemonState.off_screen;
    [SerializeField] int speedNormal = 4;
    [SerializeField] int speedExit = 8;
    [SerializeField] float idleCountdown = 2f;
    [SerializeField] int damage = 5;
    [SerializeField] float attackTimeInterval = 0.75f;
    #pragma warning restore 0649    // restore the warning for SerializeFields that are assigned within the Unity Editor
    private float attackCountdown = 0.5f;
    private GameObject[] bldgList;
    private int speed;
    private GameObject currentTarget = null;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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

    // Shuffle algorithm for arrays to target misc buildings first
    private static void Shuffle<T>(T[] origArray) {
        // Debug.Log("SHUFFLING BUILDINGS...");
        System.Random r = new System.Random();

        for (int i=0; i<origArray.Length - 1; i++) {
            int loc = r.Next(0, origArray.Length);
            // Debug.Log("Swapping " + origArray[i] + " with " + origArray[loc]);
            T temp = origArray[i];
            origArray[i] = origArray[loc];
            origArray[loc] = temp;
        }
    }

    public void FindTarget() {
        bldgList = GameObject.FindGameObjectsWithTag("Building");
        Shuffle(bldgList);

        // find building target
        // first search for vice bldgs top priority
        foreach (GameObject bldg in bldgList) {
            // Debug.Log(demonType.ToString() + " Demon considering attacking: " + bldg.GetComponent<BuildingInfo>().BuildingType.ToString() );
            if (bldg.GetComponent<BuildingInfo>().BuildingType == BuildingType.Vice) {
                // Debug.Log("DEMON WANTS THAT VICE!!");
                currentTarget = bldg;
                demonState = DemonState.moving;
                break;            
            }
        }
        // no vice found so now checking for matching type
        if (currentTarget == null) {
            foreach (GameObject bldg in bldgList) {
                if (bldg.GetComponent<BuildingInfo>().BuildingType.ToString() == demonType.ToString() ) {
                // Debug.Log("DEMON GOING TOWARDS " + demonType.ToString() );
                    currentTarget = bldg;
                    demonState = DemonState.moving;
                    break;
                }
            }
        }
        // no vice or matching so just target whatever
        if (currentTarget == null) {
            foreach (GameObject bldg in bldgList) {
            // Debug.Log("No match found, attack whatever...\nMOVING TOWARDS BUILDING TYPE " + bldg.GetComponent<BuildingInfo>().BuildingType.ToString() );
            currentTarget = bldg;
            demonState = DemonState.moving;
            break;
            }
        }
    }

    public void MoveTowardsTarget() { 

        Vector3 direction = (currentTarget.transform.position - transform.position);

        animator.SetBool("attacking", false);     
        animator.SetFloat("xDirection", direction.x);
        animator.SetFloat("yDirection", direction.y);

        // check if reached target
        if (direction.magnitude <= .11) {
            // reached target.  Start attacking
            demonState = DemonState.attacking;
            animator.SetBool("attacking", true);     
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
            attackCountdown = attackTimeInterval;

            if (currentTarget.GetComponentInChildren<BuildingHealth>().CurrentHealth <= 0) {
                // building smashed!  Retarget to new building
                demonState = DemonState.targeting;
            }
        }
    }

    public void EndNightPhase() {
        demonState = DemonState.exiting;
        speed = speedExit;
        currentTarget = new GameObject();
        currentTarget.transform.position = new Vector3(999, 999, 0);
        Destroy(this, 5f);
    }

    private void BeGoneDemon() {
        Vector3 direction = (currentTarget.transform.position - transform.position);
        direction.Normalize();
        transform.Translate(direction*speed * Time.fixedDeltaTime); 
    }
}
