using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class ShieldSoundManager : MonoBehaviour
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void PlayShieldUpSound()
    {
      //  AudioManager.PlaySound(Sounds.ShieldUp);
    }
    
    public void PlayShieldPopSound()
    {
       // AudioManager.PlaySound(Sounds.ShieldPop);
       soundManager.PlayOneSound("Shield Pop");
    }
}
