using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class PlayerSoundManager : MonoBehaviour
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void PlayFootstepsSound()
    {
        soundManager.PlayArraySound();
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
