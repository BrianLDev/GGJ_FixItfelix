using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Characters : NightTimeListener
{
    public static Characters instance;

    public static int lysID = 1;
    public static int jacqueID = 2;
    public static int steelID = 3;
    public static int angelID = 4;
    public static bool lysEnabled = false;
    public static bool jacqueEnabled = false;
    public static bool steelEnabled = false;
    public static bool angelEnabled = false;
    public Texture lysSprite;
    public Texture jacqueSprite;
    public Texture steelSprite;
    public Texture angelSprite;
    public int characterID = 0;

    public RawImage characterImage;

    public GameObject characterCanvas;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        characterCanvas = transform.Find("CharacterCanvas").gameObject;
        characterImage = characterCanvas.transform.Find("Image").GetComponent<RawImage>();

        //StartCharacter(steelID);
    }

    public void StartCharacter(int idToStart)
    {
        print("start" + idToStart);
        characterCanvas.SetActive(true);
        if (idToStart == lysID)
        {
            characterImage.texture = lysSprite;
        }
        else if (idToStart == jacqueID)
        {
            characterImage.texture = jacqueSprite;
        }
        else if (idToStart == steelID)
        {
            characterImage.texture = steelSprite;
        }
        else if (idToStart == angelID)
        {
            characterImage.texture = angelSprite;
        }

        characterID = idToStart;
        characterCanvas.SetActive(true);
    }

    public void AcceptCharacter()
    {
        if (characterID == lysID)
        {
            lysEnabled = true;
        }
        else if (characterID == jacqueID)
        {
            jacqueEnabled = true;
        }
        else if (characterID == steelID)
        {
            steelEnabled = true;
        }
        else if (characterID == angelID)
        {
            angelEnabled = true;
        }

        characterCanvas.SetActive(false);
    }

    public void DeclineCharacter()
    {
        characterID = 0;
        characterCanvas.SetActive(false);
    }

    public override void StartNewDay(DayNightCycle cycle)
    {
        if (cycle.GetCurrentDay() % 2 == 1)
        {
            int random = Random.Range(0, 3);
            if (lysEnabled)
            {
                random = random + 1;
            }

            if (jacqueEnabled)
            {
                random = random + 1;
            }

            if (angelEnabled)
            {
                random = random + 1;
            }

            if (!angelEnabled && random >= 3)
            {
                StartCharacter(angelID);
            }
            else if (!lysEnabled && random >= 2)
            {
                StartCharacter(lysID);
            }
            else if (!steelEnabled && random >= 1)
            {
                StartCharacter(steelID);
            }
            else if (!jacqueEnabled)
            {
                StartCharacter(jacqueID);
            }
        }
    }

    public override void StartNewNight(DayNightCycle cycle)
    {
        // unneeded
    }
}
