using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    public MeterScript meter;

    private float myMind;
    private float myBody;
    private float mySoul;

    // Start is called before the first frame update
    void Start()
    {
        myMind = 2.0f;
        myBody = 5.0f;
        mySoul = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMind(float addToMind)
    {
        myMind += addToMind;
        meter.ChangeStateOfMind(addToMind / 10.0f);
    }
}
