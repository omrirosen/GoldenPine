using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTutorial : MonoBehaviour
{
    [SerializeField] private GameObject bubbleTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bubbleTutorialCanvas.SetActive(true);
        }
        
    }

    /* private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("EndTutorial", 1f);
        }
        
    }*/
    

    void EndTutorial()
    {
        bubbleTutorialCanvas.SetActive(false);
    }
}
