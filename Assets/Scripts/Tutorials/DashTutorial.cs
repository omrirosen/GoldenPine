using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorial : MonoBehaviour
{
    [SerializeField] private GameObject dashTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        dashTutorialCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Invoke("EndTutorial", 1f);
    }
    

    void EndTutorial()
    {
        dashTutorialCanvas.SetActive(false);
    }
}
