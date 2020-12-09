using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject devTeleportPos1;
    [SerializeField] GameObject devTeleportPos2;
    [SerializeField] GameObject Player;
    private static GameManager gameManagerInstance;
    public bool passedCertainCheckpoint = false;

    public Vector2 lastCheckpointReached;
    

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }
    
    private void Update()
    {
        QuitGame();
        //DebugPositions();
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
            Player.transform.position = devTeleportPos1.transform.position;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            Player.transform.position = devTeleportPos2.transform.position;
        }
    }

    public void populateRefrences()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        devTeleportPos1 = GameObject.FindWithTag("DevPos1");
        devTeleportPos2 = GameObject.FindWithTag("DevPos2");
    }
}
