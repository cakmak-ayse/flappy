using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class logic_script : MonoBehaviour
{
    public Text score ;
    public int count = 0;
    public GameObject gameOverScreen;
    public GameObject startScreen;
    public bool gameBegan = false;

    [ContextMenu("Increment Score")]    // useful for testing
    public void incrementCount(){
        count++;
        score.text = count.ToString();
    }

    public void RestrartThisScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    [ContextMenu("Game Over Screen Activate")]
    public void GameOverSceneActivate(){
        gameOverScreen.SetActive(true);
    }

    [ContextMenu("Start Screen Deactivate")]
    public void StartSceneDectivate(){
        startScreen.SetActive(false);
        this.gameBegan = true;
    }

}
