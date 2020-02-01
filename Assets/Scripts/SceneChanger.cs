using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string mainGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene(mainGame);
    }

    public void LoadTitleScreen()
    {

    }

    public void LoadEndGame()
    {

    }
}
