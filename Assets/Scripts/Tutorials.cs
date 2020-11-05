using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    [SerializeField] private GameObject jumpTutorialCanvas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        jumpTutorialCanvas.SetActive(true);
    }
}
