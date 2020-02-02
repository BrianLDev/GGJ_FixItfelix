using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string titleText = "[PH] Title";
    public string bodyText = "[PH] Body description thing";
    public int cost = 0;
    public int benefit = 0;

    public void OnMouseEnter()
    {
        Tooltip.ShowTooltip(this.gameObject, titleText, bodyText, cost, benefit);
    }

    public void OnMouseExit()
    {
        Tooltip.HideTooltip();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        print("OPEn");
        Tooltip.ShowTooltip(this.gameObject, titleText, bodyText, cost, benefit);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        print("OPEx");
        Tooltip.HideTooltip();
    }
}
