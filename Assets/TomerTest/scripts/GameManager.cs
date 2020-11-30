using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    [SerializeField] GameObject Player;
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

    private void Update()
    {
        QuitGame();
        DebugPositions();
    }

    private void DebugPositions()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Player.transform.position = pos1.position;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            Player.transform.position = pos2.position;
        }
    }
}
