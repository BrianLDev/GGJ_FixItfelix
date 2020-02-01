using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    public GameObject meter;

    private float myMind;
    private float myBody;
    private float mySoul;
    private MeterScript meterScript;

    // Start is called before the first frame update
    void Start()
    {
        myMind = 2.0f;
        myBody = 5.0f;
        mySoul = 3.0f;
        meterScript = meter.GetComponent<MeterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMind(float addToMind)
    {
        myMind += addToMind;
        meterScript.ChangeStateOfMind(addToMind / 100.0f);
    }

    public void UpdateBody(float addToBody)
    {
        myBody += addToBody;
        meterScript.ChangeStateOfBody(addToBody / 100.0f);
    }

    public void UpdateSoul(float addToSoul)
    {
        mySoul += addToSoul;
        meterScript.ChangeStateOfSoul(addToSoul / 100.0f);
    }
}
