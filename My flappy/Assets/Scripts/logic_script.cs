using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// Handles game logic including score management, game states, and data persistence
public class logic_script : MonoBehaviour, InterfaceDataPersistence
{
    // Public fields for UI elements and game state
    public Text score;
    public GameObject gameOverScreen;
    public GameObject startScreen;
    public Button restartButton;

       // Private fields for score and game state
    public int count = 0;
    private int highScore = 0;
    public bool gameBegan = false;
    private static logic_script _instance;

    //making logic a singleton and stopping it from gettong reset at scene reload
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Checking for high scores before scene reloeds or app closes
    private void OnDestroy(){  
        CheckHighscore();
    }
    // Score management

    [ContextMenu("Increment Score")]    // Useful for testing
    public void IncrementCount()
    {
        count++;
        score.text = count.ToString();
    }

    // Game state management

    // Restart the current scene
    public void RestartThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ContextMenu("Game Over Screen Activate")]
    public void GameOverSceneActivate()
    {
        gameOverScreen.SetActive(true);
    }

    [ContextMenu("Check Highscore")]
    public void CheckHighscore()
    {
        if (count > highScore)
        {
            highScore = count;
        }
        Debug.Log("Current Highscore = " + highScore);
    }

    [ContextMenu("Start Screen Deactivate")]
    public void StartScreenDeactivate()
    {
        startScreen.SetActive(false);
        gameBegan = true;
    }

    // Data persistence implementation

    // Load data from persistent storage
    public void LoadData(GameData GData)
    {
        highScore = GData.highScore;
    }

    // Save data to persistent storage
    public void SaveData(ref GameData GData)
    {
        GData.highScore = highScore;
    }
    
    // Method called on scene load
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign game objects here
        gameOverScreen = FindGameObjectByName("GameOverScreen");
        startScreen = FindGameObjectByName("Start_screen");
        restartButton = FindGameObjectByName("RestartButton").GetComponent<Button>();

        // Reassign button events
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners(); // Clear existing listeners
            restartButton.onClick.AddListener(RestartThisScene); // Add your method
        }
    }

    // Method to find game objects by name, including inactive objects
private GameObject FindGameObjectByName(string name)
{
    // Search among all root objects in the scene
    GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

    foreach (GameObject rootObject in rootObjects)
    {
        GameObject foundObject = FindInChildren(rootObject.transform, name);
        if (foundObject != null)
        {
            return foundObject;
        }
    }

    Debug.LogWarning($"GameObject with name {name} not found in the scene.");
    return null;
}

// Recursive helper method to search for a child object by name
private GameObject FindInChildren(Transform parent, string name)
{
    // Check if the current object matches the name
    if (parent.name == name)
    {
        return parent.gameObject;
    }

    // Recursively check all children
    foreach (Transform child in parent)
    {
        GameObject foundObject = FindInChildren(child, name);
        if (foundObject != null)
        {
            return foundObject;
        }
    }

    return null;
}
}
