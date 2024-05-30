using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class logic_script : MonoBehaviour
{
    public Text score;
    public GameObject gameOverScreen;
    public GameObject startScreen;
    public Button restartButton;

    public int count = 0;
    private int highScore = 0;
    public bool gameBegan = false;
    private static logic_script _instance;

    // private void Awake()
    // {
    //     if (_instance != null && _instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         _instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    // private void OnDestroy()
    // {
    //     CheckHighscore();
    // }

    [ContextMenu("Increment Score")]
    public void IncrementCount()
    {
        count++;
        score.text = count.ToString();
    }

    public void RestartThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ContextMenu("Game Over Screen Activate")]
    public void GameOverSceneActivate()
    {
        gameOverScreen.SetActive(true);
        gameBegan = false;
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
        gameOverScreen = FindGameObjectByName("GameOverScreen");
        startScreen = FindGameObjectByName("Start_screen");
        restartButton = FindGameObjectByName("RestartButton").GetComponent<Button>();

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartThisScene);
        }
    }

    private GameObject FindGameObjectByName(string name)
    {
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

    private GameObject FindInChildren(Transform parent, string name)
    {
        if (parent.name == name)
        {
            return parent.gameObject;
        }

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
