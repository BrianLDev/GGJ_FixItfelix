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

    // Start is called before the first frame update
    void Start()
    {
        pss = playerStatsObj.GetComponent<PlayerStatsScript>();
        pss.SetMind(startPlayerMind);
        pss.SetBody(startPlayerBody);
        pss.SetSoul(startPlayerSoul);
    }

    // Update is called once per frame
    void Update()
    {
        if (pss.GetBody() <= 0 && dnc.IsNightTime())
        {
            LoseGame();
        }
        if (pss.GetSoul() >= 2000)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        sc.LoadWinGame();
    }

    private void LoseGame()
    {
        sc.LoadLoseGame();
    }
}
