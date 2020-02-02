using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // to load scenes

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;  // public instance of this GameManager that all scripts can access (singleton)
    public enum GameState {MainMenu = 0, PlayingGame = 1, Paused = 3, GameOver = 4}
    public GameState gameState = GameState.MainMenu;

    void Awake() {
        if (instance == null)   // Check if instance already exists
            instance = this;    // if not, set instance to this

        else if (instance != this)  // If instance already exists and it's not this:
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    

        //DontDestroyOnLoad(gameObject);  // Sets this to not be destroyed when reloading scene
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame() {
        /// TODO: This is where we initialize anything when a new game starts
        Debug.Log("It begins...");
        gameState = GameState.PlayingGame;
    }

    public void GameOver() {
        /// TODO: This is where we initialize anything when a new game starts
        Debug.Log("Game Over man...");
        gameState = GameState.GameOver;
    }


}
