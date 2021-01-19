using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class PlayerSoundManager : MonoBehaviour
{
    private SoundManager soundManager;
    public bool inCave = false;
    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void PlayFootstepsSound()
    {
        if (!inCave)
        {
            soundManager.PlayFootStepsArray();
        }
        else
        {
            soundManager.PlayCaveFootStepsArray();
        }
       // AudioManager.PlaySound(Sounds.Footsteps);
    }

    public void PlayDashSound()
    {
       //AudioManager.PlaySound(Sounds.Dash);
    }

    public void PlayJumpSound()
    {
        //AudioManager.PlaySound(Sounds.Jump);
    }
}
