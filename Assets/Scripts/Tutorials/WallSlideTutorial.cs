using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideTutorial : MonoBehaviour
{
    [SerializeField] private GameObject wallslideTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wallslideTutorialCanvas.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("EndTutorial", 1f);
        }
        
    }
    

    void EndTutorial()
    {
        wallslideTutorialCanvas.SetActive(false);
    }
}
