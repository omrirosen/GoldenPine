using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorial : MonoBehaviour
{
    [SerializeField] private GameObject dashTutorialCanvas;
    private SoundManager soundManager;
    bool PlayedSound = false;
    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dashTutorialCanvas.SetActive(true);
            if (!PlayedSound)
            {
                soundManager.PlayOneSound("Tutorials Opening");
                PlayedSound = true;
            }
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
        dashTutorialCanvas.SetActive(false);
    }
}
