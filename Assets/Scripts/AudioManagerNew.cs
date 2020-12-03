using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Random = System.Random;

public class AudioManagerNew : MonoBehaviour
{
  public Sounds[] sounds;
  public static AudioManagerNew instance; 

  private void Awake()
  {
    DontDestroyOnLoad(gameObject);
    
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }
   
    foreach (var s in sounds)
    {
      s.audioSource = gameObject.AddComponent<AudioSource>();
     // s.audioSource.clip = s.audioClip[UnityEngine.Random.Range(0, s.audioClip.Length)];
      s.audioSource.volume = s.volume;
      s.audioSource.pitch = s.pitch;
      s.audioSource.outputAudioMixerGroup = s.audioMixerGroup;
      s.audioSource.loop = s.loop;
    }
  }
  
  public void PlaySound(string name)
  {
    Sounds s = Array.Find(sounds, sound => sound.name == name);
     s.audioSource.Play();
     if (s == null)
     {
       Debug.LogWarning("Sound" + name + "not found");
       return;
     }
     
  }
  //How to play sounds from other scripts:
  // FindObjectOfType<AudioManager>().Play("string of clip name");
}
