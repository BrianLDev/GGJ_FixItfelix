using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateScript : MonoBehaviour
{
    public float startPlayerMind;
    public float startPlayerBody;
    public float startPlayerSoul;
    public GameObject playerStatsObj;
    public GameObject gameManager;
    public GameObject sceneManagement;

    private PlayerStatsScript pss;
    private DayNightCycle dnc;
    private SceneChanger sc;
    private AudioManagerScript ams;
    private GameObject[] buildings;
    private bool playingGame;

    // Start is called before the first frame update
    void Start()
    {
        pss = playerStatsObj.GetComponent<PlayerStatsScript>();
        pss.SetMind(startPlayerMind);
        pss.SetBody(startPlayerBody);
        pss.SetSoul(startPlayerSoul);

        dnc = gameManager.GetComponentInChildren<DayNightCycle>();

        sc = sceneManagement.GetComponent<SceneChanger>();

        ams = gameManager.GetComponentInChildren<AudioManagerScript>();

        playingGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playingGame)
        {
            if (pss.GetBody() <= 0 && dnc.IsNightTime())
            {
                playingGame = false;
                LoseGame();
            }
            else if (pss.GetSoul() >= 2000)
            {
                playingGame = false;
                WinGame();
            }
        }
    }

    private void FixedUpdate()
    {
        GameObject[] bldgList = GameObject.FindGameObjectsWithTag("Building");

        
        float totalBldgHealth = 0.0f;
        //float buildingCost = 0.0f;
        // find building target
        foreach (GameObject bldg in bldgList)
        {
            BuildingHealth bh = bldg.GetComponent<BuildingHealth>();
            //BuildingInfo bi = bldg.GetComponent<BuildingInfo>();
            totalBldgHealth += bh.CurrentHealth;
            //buildingCost += bi.BaseCost;
        }

        pss.SetBody(totalBldgHealth);
        //pss.SetMind(pss.GetMind() - buildingCost);
    }

    public bool IsPlayingGame()
    {
        return playingGame;
    }

    public void UpdatePlayerMind(float mindNum)
    {
        pss.UpdateMind(mindNum);
    }

    private void WinGame()
    {
        ams.musicCanPlay = false;
        sc.LoadWinGame();
    }

    private void LoseGame()
    {
        ams.musicCanPlay = false;
        sc.LoadLoseGame();
    }
}
