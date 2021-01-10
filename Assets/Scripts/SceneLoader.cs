using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 1f;
    private GameManager gameManager;



    private void Awake()
    {
        //gameManager = FindObjectOfType<GameManager>();
    }

    public void LoadSceneByName(string nameofSceneToLoad)
    {
        StartCoroutine(SceneNameCor(nameofSceneToLoad));
        /*if (gameManager != null)
        {
            gameManager.ResetGame();
        }*/
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCor(SceneManager.GetActiveScene().buildIndex + 1));
        /*if (gameManager != null)
        {
            gameManager.ResetGame();
        }*/
    }

    public void RestartLastLevel()
    {
        StartCoroutine(LoadLevelCor(SceneManager.GetActiveScene().buildIndex));
        /*if (gameManager != null)
        {
            gameManager.ResetGame();
        }*/
    }

    public void QuitGame()
    {
        print("quit");
        Application.Quit();
    }



    IEnumerator LoadLevelCor(int levelIndex)
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator SceneNameCor(string nameofSceneToLoad)
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        SceneManager.LoadScene(nameofSceneToLoad);
    }

}