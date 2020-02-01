using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateScript : MonoBehaviour
{
    public float startPlayerMind;
    public float startPlayerBody;
    public float startPlayerSoul;
    public GameObject playerStatsObj;

    private PlayerStatsScript pss;
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
        
    }
}
