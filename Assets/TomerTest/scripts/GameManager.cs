using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform devTeleportPos1;
    [SerializeField] Transform devTeleportPos2;
    [SerializeField] GameObject Player;
    private static GameManager gameManagerInstance;
    public bool passedCertainCheckpoint = false;

    public Vector2 lastCheckpointReached;
    

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        QuitGame();
        DebugPositions();
    }

    public void ResetScene()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void QuitGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }
    }
    
    private void DebugPositions()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Player.transform.position = devTeleportPos1.position;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            Player.transform.position = devTeleportPos2.position;
        }
    }
    
    
}
