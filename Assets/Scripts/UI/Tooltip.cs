using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip tooltip;

    public static float costBenefitHeight = 30f;

    public float defaultHeight;

    private Text title;
    private Text body;

    private GameObject cost;
    private GameObject benefit;
    private Text costText;
    private Text benefitText;

    private RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        tooltip = this;

        rectTransform = GetComponent<RectTransform>();

        defaultHeight = rectTransform.rect.height;

        title = transform.Find("Title").GetComponent<Text>();
        body = transform.Find("Text").GetComponent<Text>();
        cost = transform.Find("Cost").gameObject;
        costText = cost.transform.Find("Text").GetComponent<Text>();
        benefit = transform.Find("Benefit").gameObject;
        benefitText = cost.transform.Find("Text").GetComponent<Text>();

        //ShowTooltip("Test Title", "This is a test tooltip", -200, 50);
        //ShowTooltip("Test title 2", "More testing!");
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x + (rectTransform.rect.width / 2.0f), Input.mousePosition.y + (rectTransform.rect.height / 2.0f));
    }

    public static void ShowTooltip(string titleText, string bodyText, int costAmount = 0, int benefitAmount = 0)
    {
        tooltip.title.text = titleText;
        tooltip.body.text = bodyText;

        bool hasCost = costAmount != 0;
        tooltip.cost.SetActive(hasCost);
        tooltip.benefit.SetActive(hasCost);
        if (hasCost)
        {
            tooltip.costText.text = costAmount.ToString();
            tooltip.benefitText.text = benefitAmount.ToString();
            //tooltip.rectTransform.rect.Set(tooltip.rectTransform.rect.x, tooltip.rectTransform.rect.y, tooltip.rectTransform.rect.width, tooltip.defaultHeight);
            tooltip.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tooltip.defaultHeight);
        }
        else
        {
            print("no cost");
            //tooltip.rectTransform.rect.Set(tooltip.rectTransform.rect.x, tooltip.rectTransform.rect.y, tooltip.rectTransform.rect.width, tooltip.defaultHeight - costBenefitHeight);
            //tooltip.rectTransform.rect.Set(tooltip.rectTransform.rect.x, tooltip.rectTransform.rect.y, 167, 76);
            tooltip.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tooltip.defaultHeight - costBenefitHeight);
        }

        tooltip.gameObject.SetActive(true);
    }

    public static string GetTitle()
    {
        return tooltip.title.text;
    }

    public static void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
}
