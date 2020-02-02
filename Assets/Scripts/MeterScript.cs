using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterScript : MonoBehaviour
{
    //private float barDisplay;

    public string mindTitle = "Mind";
    public string bodyTitle = "Body";
    public string soulTitle = "Soul";
    public string mindTooltipText = "Spend mind to repair buildings and upgrade buildings. (Library)";
    public string bodyTooltipText = "Restore and repair buildings to increase body. (All buildings, Gym)";
    public string soulTooltipText = "Increase your soul value to repair your life. (Ampitheater)";

    private Vector2 posMind;
    private Vector2 sizeMind;
    private float currentMindState;
    private float currentMindPos;

    private Vector2 posBody;
    private Vector2 sizeBody;
    private float currentBodyState;
    private float currentBodyPos;

    private Vector2 posSoul;
    private Vector2 sizeSoul;
    private float currentSoulState;
    private float currentSoulPos;

    //public float newMindVal;
    //public float newBodyVal;
    //public float newSoulVal;
    public Texture2D progressBarEmpty;
    //public Texture2D progressBarFull;

    void Awake()
    {
        currentMindPos = 0.2f;
        currentMindState = 0.2f;
        posMind = new Vector2(Screen.width * currentMindPos, Screen.height * 0.8f);
        sizeMind = new Vector2(Screen.width * currentMindState, Screen.height * 0.1f);

        currentBodyPos = 0.4f;
        currentBodyState = 0.2f;
        posBody = new Vector2(Screen.width * currentBodyPos, Screen.height * 0.8f);
        sizeBody = new Vector2(Screen.width * currentBodyState, Screen.height * 0.1f);

        currentSoulPos = 0.599f;
        currentSoulState = 0.2f;
        posSoul = new Vector2(Screen.width * currentSoulPos, Screen.height * 0.8f);
        sizeSoul = new Vector2(Screen.width * currentSoulState, Screen.height * 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeStateOfMind(newMindVal);
        //ChangeStateOfBody(newBodyVal);
        //ChangeStateOfSoul(newSoulVal);
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            //Mind bar
            GUI.BeginGroup(new Rect(posMind.x, posMind.y, sizeMind.x, sizeMind.y), "M");
            GUI.Box(new Rect(0, 0, sizeMind.x, sizeMind.y), new GUIContent(progressBarEmpty, mindTooltipText));
            GUI.EndGroup();

            //Body bar
            GUI.BeginGroup(new Rect(posBody.x, posBody.y, sizeBody.x, sizeBody.y), "B");
            GUI.Box(new Rect(0, 0, sizeBody.x, sizeBody.y), new GUIContent(progressBarEmpty, bodyTooltipText));
            GUI.EndGroup();

            //Soul bar
            GUI.BeginGroup(new Rect(posSoul.x, posSoul.y, sizeSoul.x, sizeSoul.y), "S");
            GUI.Box(new Rect(0, 0, sizeSoul.x, sizeSoul.y), new GUIContent(progressBarEmpty, soulTooltipText));
            GUI.EndGroup();


            string tooltipTitle = Tooltip.GetTitle();

            if (GUI.tooltip == mindTooltipText)
            {
                Tooltip.ShowTooltip(mindTitle, GUI.tooltip);
            }
            else if (GUI.tooltip == bodyTooltipText)
            {
                Tooltip.ShowTooltip(bodyTitle, GUI.tooltip);
            }
            else if (GUI.tooltip == soulTooltipText)
            {
                Tooltip.ShowTooltip(soulTitle, GUI.tooltip);
            }
            else if (tooltipTitle == mindTitle || tooltipTitle == bodyTitle || tooltipTitle == soulTitle)
            {
                Tooltip.HideTooltip();
            }
        }
    }

    //Mind methods
    public void SetNewStateOfMind(float newMindState)
    {
        MoveMindBar(newMindState - currentMindPos);
        currentMindState = newMindState;
        sizeMind = new Vector2(Screen.width * currentMindState, Screen.height * 0.1f);
    }

    public void UpdateStateOfMind(float newMindState)
    {
        currentMindState += newMindState;
        sizeMind = new Vector2(Screen.width * currentMindState, Screen.height * 0.1f);
        MoveMindBar(newMindState);
    }

    private void MoveMindBar(float newMindPos)
    {
        currentMindPos -= newMindPos;
        posMind = new Vector2(Screen.width * currentMindPos, Screen.height * 0.8f);
    }

    //Body methods
    public void SetNewStateOfBody(float newBodyState)
    {

        //MoveMindBar(halfNewBodyPos);
        //MoveSoulBar(halfNewBodyPos);

        float halfNewBodyPos = newBodyState * 0.5f;
        currentBodyState = newBodyState;
        currentBodyPos = 0.5f - halfNewBodyPos;
        posBody = new Vector2(Screen.width * currentBodyPos, Screen.height * 0.8f);
        sizeBody = new Vector2(Screen.width * currentBodyState, Screen.height * 0.1f);

        currentMindPos = currentBodyPos - currentMindState;
        posMind = new Vector2(Screen.width * currentMindPos, Screen.height * 0.8f);
        currentSoulPos = currentBodyPos + currentBodyState;
        posSoul = new Vector2(Screen.width * currentSoulPos, Screen.height * 0.8f);
    }

    public void UpdateStateOfBody(float newBodyState)
    {
        currentBodyState += newBodyState;
        float halfNewBodyState = newBodyState * 0.5f;
        currentBodyPos -= halfNewBodyState;

        posBody = new Vector2(Screen.width * currentBodyPos, Screen.height * 0.8f);
        sizeBody = new Vector2(Screen.width * currentBodyState, Screen.height * 0.1f);

        MoveMindBar(halfNewBodyState);
        MoveSoulBar(halfNewBodyState);
    }

    //Soul methods
    public void SetNewStateOfSoul(float newSoulState)
    {
        //MoveSoulBar(currentSoulPos - newSoulState);
        currentSoulState = newSoulState;
        sizeSoul = new Vector2(Screen.width * currentSoulState, Screen.height * 0.1f);
    }

    public void MoveSoulBar(float newSoulPos)
    {
        currentSoulPos += newSoulPos;
        posSoul = new Vector2(Screen.width * currentSoulPos, Screen.height * 0.8f);
    }

    public void UpdateStateOfSoul(float newSoulState)
    {
        currentSoulState += newSoulState;
        sizeSoul = new Vector2(Screen.width * currentSoulState, Screen.height * 0.1f);
    }
}
