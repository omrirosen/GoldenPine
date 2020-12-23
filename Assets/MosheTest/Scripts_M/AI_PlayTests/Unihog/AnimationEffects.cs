using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects :MonoBehaviour
{
    [SerializeField] GameObject UnihogRollDust_ins;
    [SerializeField] Transform UnihogRollDust_position;
    [SerializeField] Unihog1Controller unihog;
    public bool didPlayRollDust = false;
    [SerializeField] float x;
    private EnemySoundManager enemySoundManager;

    private void Awake()
    {
        enemySoundManager = FindObjectOfType<EnemySoundManager>();
    }

    private void Update()
    {
       x = unihog.transform.localScale.x;
       
    }
    public void playRollDust()
    {
        
        if (!didPlayRollDust)
        {
            
            didPlayRollDust = true;
            if (x > 0f)
            {
                GameObject temp = Instantiate(UnihogRollDust_ins, UnihogRollDust_position.position, Quaternion.identity);
                temp.GetComponentInChildren<SpriteRenderer>().flipX = true;

            }
            else
            {
                GameObject temp = Instantiate(UnihogRollDust_ins, UnihogRollDust_position.position, Quaternion.identity);
                temp.GetComponentInChildren<SpriteRenderer>().flipX = false;
                
            }

        }
        
        
    }

    public void PlayIdleSoundOne()
    {
        enemySoundManager.PlayOneSound("Idle 1");
    }

    public void PlayIdleSoundTwo()
    {
        enemySoundManager.PlayOneSound("Idle 2");
    }

    public void PlayEvaporateSound()
    {
        enemySoundManager.PlayOneSound("Evaporate");
    }

    public void PlayRollSound()
    {
        enemySoundManager.PlayOneSound("Roll Attack");
    }

    



}
