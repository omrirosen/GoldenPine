using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    [SerializeField] private GameObject jumpTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        jumpTutorialCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Invoke("EndTutorial", 1f);
    }
    

    void EndTutorial()
    {
        jumpTutorialCanvas.SetActive(false);
    }
}
