using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterScript : MonoBehaviour
{
    //private float barDisplay;

    private Vector2 posMind;
    private Vector2 sizeMind;
    private float currentMindState;
    private float currentMindPos;

    private Vector2 posBody;
    private Vector2 sizeBody;

    private Vector2 posSoul;
    private Vector2 sizeSoul;

    public float newMindPosVal;
    public Texture2D progressBarEmpty;
    //public Texture2D progressBarFull;
 
    // Start is called before the first frame update
    void Start()
    {
        //barDisplay = 0.5f;
        currentMindPos = 0.2f;
        currentMindState = 0.2f;

        posMind = new Vector2(Screen.width * currentMindPos, Screen.height * 0.8f);
        posBody = new Vector2(Screen.width * 0.4f, Screen.height * 0.8f);
        posSoul = new Vector2(Screen.width * 0.599f, Screen.height * 0.8f);

        sizeMind = new Vector2(Screen.width * (currentMindPos - currentMindState), Screen.height * 0.1f);
        sizeBody = new Vector2(Screen.width * 0.2f, Screen.height * 0.1f);
        sizeSoul = new Vector2(Screen.width * 0.2f, Screen.height * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveMindBar(newMindPosVal);
    }

    void OnGUI()
    {

        //Mind bar
        GUI.BeginGroup(new Rect(posMind.x, posMind.y, sizeMind.x, sizeMind.y));
        GUI.Box(new Rect(0, 0, sizeMind.x, sizeMind.y), progressBarEmpty);
        GUI.EndGroup();

        //Body bar
        GUI.BeginGroup(new Rect(posBody.x, posBody.y, sizeBody.x, sizeBody.y));
        GUI.Box(new Rect(0, 0, sizeBody.x, sizeBody.y), progressBarEmpty);
        GUI.EndGroup();

        //Soul bar
        GUI.BeginGroup(new Rect(posSoul.x, posSoul.y, sizeSoul.x, sizeSoul.y));
        GUI.Box(new Rect(0, 0, sizeSoul.x, sizeSoul.y), progressBarEmpty);
        GUI.EndGroup();
    }

    public void MoveMindBar(float newMindPos)
    {
        currentMindPos -= newMindPos;
        posMind = new Vector2(Screen.width * currentMindPos, Screen.height * 0.8f);
        sizeMind = new Vector2(Screen.width * currentMindState, Screen.height * 0.1f);
    }

    public void ChangeStateOfMind(float newMindState)
    {

    }
}
