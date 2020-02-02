using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipListener : MonoBehaviour
{
    public string titleText;
    public string bodyText;
    public int cost = 0;
    public int benefit = 0;

    public void OnMouseEnter()
    {
        Tooltip.ShowTooltip(titleText, bodyText, cost, benefit);
    }

    public void OnMouseExit()
    {
        Tooltip.HideTooltip();
    }
}
