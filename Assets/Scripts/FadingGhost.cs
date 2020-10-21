using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingGhost : MonoBehaviour
{
    [SerializeField] private float ghostDelay;
    [SerializeField] private float destroyDelay = 1f;
    public GameObject characterGhost;
    private float ghostDelaySecondes;
    public bool createGhost = false;

    private void Start()
    {
        ghostDelaySecondes = ghostDelay;
    }

    private void Update()
    {
        if (createGhost)
        {
            if (ghostDelaySecondes > 0)
            {
                ghostDelaySecondes -= Time.deltaTime;
            
            }
            else
            {
                //generate a ghost
                GameObject currentGhost = Instantiate(characterGhost, transform.position, Quaternion.identity);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.rotation = transform.rotation;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySecondes = ghostDelay;
                Destroy(currentGhost, destroyDelay);

            }  
        }
        
    }
}
