using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideTutorial : MonoBehaviour
{
    [SerializeField] private GameObject wallslideTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        wallslideTutorialCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Invoke("EndTutorial", 1f);
    }
    

    void EndTutorial()
    {
        wallslideTutorialCanvas.SetActive(false);
    }
}
