using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDemo : MonoBehaviour
{
    private SceneLoader SceneLoader;

    
    
    
    private void Awake()
    {
        SceneLoader = FindObjectOfType<SceneLoader>();
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneLoader.LoadNextLevel();
        }
    }

}
