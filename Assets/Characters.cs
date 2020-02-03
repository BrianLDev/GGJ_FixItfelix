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

    public float dayTime = 0.0f;
    public bool shouldAddCharacter = false;

    private AudioManagerScript audioManager;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        characterCanvas = transform.Find("CharacterCanvas").gameObject;
        characterImage = characterCanvas.transform.Find("Image").GetComponent<RawImage>();

        audioManager = this.transform.parent.GetComponentInChildren<AudioManagerScript>();
    }

    public void FixedUpdate()
    {
        if (DayNightCycle.instance.IsNightTime())
        {
            dayTime = 0.0f;
            shouldAddCharacter = false;
        }
        else
        {
            dayTime = dayTime + Time.fixedDeltaTime;
            if (dayTime > 3.0f && characterID != 0 && shouldAddCharacter)
            {
                StartCharacter(characterID);
                shouldAddCharacter = false;
            }
        }
    }

    public void StartCharacter(int idToStart)
    {
        characterCanvas.SetActive(true);
        if (idToStart == lysID)
        {
            characterImage.texture = lysSprite;
            audioManager.PlayLysTheme();
        }
        else if (idToStart == jacqueID)
        {
            characterImage.texture = jacqueSprite;
            audioManager.PlayJacqueTheme();
        }
        else if (idToStart == steelID)
        {
            characterImage.texture = steelSprite;
            audioManager.PlaySteelTheme();
        }
        else if (idToStart == angelID)
        {
            characterImage.texture = angelSprite;
            audioManager.PlayAngelTheme();
        }

        audioManager.StopDaySong();

        characterID = idToStart;
        characterCanvas.SetActive(true);
    }

    public void AcceptCharacter()
    {
        if (characterID == lysID)
        {
            lysEnabled = true;
            audioManager.StopLysTheme();
        }
        else if (characterID == jacqueID)
        {
            jacqueEnabled = true;
            audioManager.StopJacqueTheme();
            BuildingManager.instance.OnHealthBonusMayHaveChanged();
            PlayerStatsScript.instance.UpdateMind(-200f);
        }
        else if (characterID == steelID)
        {
            steelEnabled = true;
            audioManager.StopSteelTheme();
            PlayerStatsScript.instance.UpdateSoul(-100f);
        }
        else if (characterID == angelID)
        {
            angelEnabled = true;
            audioManager.StopAngelTheme();
        }
        audioManager.PlayDayTheme();
        characterCanvas.SetActive(false);
    }

    public void DeclineCharacter()
    {
        if (characterID == lysID)
        {
            audioManager.StopLysTheme();
        }
        else if (characterID == jacqueID)
        {
            audioManager.StopJacqueTheme();
        }
        else if (characterID == steelID)
        {
            audioManager.StopSteelTheme();
        }
        else if (characterID == angelID)
        {
            audioManager.StopAngelTheme();
        }
        audioManager.PlayDayTheme();
        characterID = 0;
        characterCanvas.SetActive(false);
    }

    public override void StartNewDay(DayNightCycle cycle)
    {
        if (cycle.GetCurrentDay() % 3 == 2)
        {
            List<int> characters = new List<int>();
            if (!angelEnabled)
            {
                characters.Add(angelID);
            }

            if (!jacqueEnabled)
            {
                characters.Add(jacqueID);
            }

            if (!lysEnabled)
            {
                characters.Add(lysID);
            }

            if (!steelEnabled)
            {
                characters.Add(steelID);
            }
            
            if (characters.Count > 0)
            {
                characterID = characters[Random.Range(0, characters.Count - 1)];
                shouldAddCharacter = true;
            }
            else
            {
                characterID = 0;
                shouldAddCharacter = false;
            }
        }
    }

    public override void StartNewNight(DayNightCycle cycle)
    {
        // unneeded
    }
}
