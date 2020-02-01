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

    void Awake()
    {
        myMind = 100.0f;
        myBody = 100.0f;
        mySoul = 100.0f;
        meterScript = meter.GetComponent<MeterScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //set a new starting statistic for the mind bar
    public void SetMind(float newMind)
    {
        myMind = newMind;
        meterScript.SetNewStateOfMind(ConvertInputtoPercentage(newMind));
    }

    //add to or subtract from the mind bar
    public void UpdateMind(float addToMind)
    {
        myMind += addToMind;
        meterScript.UpdateStateOfMind(ConvertInputtoPercentage(addToMind));
    }

    public void SetBody(float newBody)
    {
        myBody = newBody;
        meterScript.SetNewStateOfBody(ConvertInputtoPercentage(newBody));
    }

    public void UpdateBody(float addToBody)
    {
        myBody += addToBody;
        meterScript.UpdateStateOfBody(ConvertInputtoPercentage(addToBody));
    }

    public void SetSoul(float newSoul)
    {
        mySoul = newSoul;
        meterScript.SetNewStateOfSoul(ConvertInputtoPercentage(newSoul));
    }

    public void UpdateSoul(float addToSoul)
    {
        mySoul += addToSoul;
        meterScript.UpdateStateOfSoul(ConvertInputtoPercentage(addToSoul));
    }

    private float ConvertInputtoPercentage(float input)
    {
        return input * 0.0002f;
    }
}
