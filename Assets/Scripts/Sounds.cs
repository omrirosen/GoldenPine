using System.Collections.Generic;
using  UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip audioClip;
    public AudioClip[]  audioClipArry;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;

    public bool loop;

    public AudioMixerGroup audioMixerGroup;
    
    [HideInInspector]
    public AudioSource audioSource;
    

}
