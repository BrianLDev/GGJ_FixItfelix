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

    private GameObject target = null;


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
        if (target)
        {
            RectTransform targetTransform = target.GetComponent<RectTransform>();
            if (targetTransform)
            {
                Debug.Log((targetTransform.rect.width / 2.0f) + ", " + target.transform.position.x + ", " + (rectTransform.rect.width / 2.0f));
                //rectTransform.rect.Set((targetTransform.rect.width / 2.0f) + targetTransform.rect.x + (rectTransform.rect.width / 2.0f), (targetTransform.rect.height / 2.0f) + targetTransform.rect.y + (rectTransform.rect.height / 2.0f), rectTransform.rect.width, rectTransform.rect.height);
                //rectTransform.rect.Set(target.transform.position.x, target.transform.position.y, rectTransform.rect.width, rectTransform.rect.height);
                transform.position = new Vector3(target.transform.position.x + (targetTransform.rect.width / 2.0f) + (rectTransform.rect.width / 3.0f), target.transform.position.y + (targetTransform.rect.height / 2.0f) + (rectTransform.rect.height / 3.0f));
            }
            else
            {
                transform.position = new Vector3(5 + target.transform.position.x + (rectTransform.rect.width / 2.0f), 5 + target.transform.position.y + (rectTransform.rect.height / 2.0f));
            }
        }
        else
        {
            transform.position = new Vector3(Input.mousePosition.x + (rectTransform.rect.width / 2.0f), Input.mousePosition.y + (rectTransform.rect.height / 2.0f));
        }
    }

    public static void ShowTooltip(GameObject gameObject = null, string titleText = "", string bodyText = "", int costAmount = 0, int benefitAmount = 0)
    {
        ShowTooltip(titleText, bodyText, costAmount, benefitAmount);
        tooltip.target = gameObject;
    }

    public static void ShowTooltip(string titleText = "", string bodyText = "", int costAmount = 0, int benefitAmount = 0)
    {
        costAmount = -costAmount;
        tooltip.target = null;

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
