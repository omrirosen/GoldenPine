using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDemo : MonoBehaviour
{
    private SceneLoader SceneLoader;
    private SceneFader sceneFader;
    
    
    
    private void Awake()
    {
        SceneLoader = FindObjectOfType<SceneLoader>();
        sceneFader = FindObjectOfType<SceneFader>();
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("loadNextScene", 1f);
            sceneFader.StartCoroutine("FadeOut");
        }
    }

    private void loadNextScene()
    {
        SceneLoader.LoadNextLevel();
    }
}
