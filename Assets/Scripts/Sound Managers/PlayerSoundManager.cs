using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class PlayerSoundManager : MonoBehaviour
{
    private AudioManagerNew audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerNew>();
    }

    public void PlayFootstepsSound()
    {
        audioManager.PlaySound("Footsteps");
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
