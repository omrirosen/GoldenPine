using UnityEngine;
using System;
using UnityEngine.Audio;

public class EnemySoundManager : MonoBehaviour
{
     public Sound[] sounds;      // store all our sounds
    public Sound[] enemyArraySounds;    // store all our music

    private int currentPlayingArrayIndex = 999; // set high to signify no song playing
   
    // a play music flag so we can stop playing music during cutscenes etc
    private bool shouldPlayEnemyArraySounds = false;
    
    
    public static EnemySoundManager instance; // will hold a reference to the first AudioManager created

    private float mvol; // Global music volume
    private float evol; // Global effects volume
    

    private void Awake()
    {

        // get preferences
        mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        evol = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);

        createAudioSources(sounds, evol);     // create sources for effects
        
    }
    void Update()
    {
       // FootStepsIndex();
       
    }

    // create sources
    private void createAudioSources(Sound[] sounds, float volume)
    {
        foreach (Sound s in sounds) {   // loop through each music/effect
            s.source = gameObject.AddComponent<AudioSource>(); // create anew audio source(where the sounds plays from in the world)
            s.source.clip = s.clip;     // the actual music/effect clip
            s.source.volume = s.volume * volume; // set volume based on parameter
            s.source.pitch = s.pitch;   // set the pitch
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.loop = s.loop;     // should it loop
            s.source.spatialBlend = s.spatialBlend;
            s.source.minDistance = s.minDistanceSound;
            s.source.maxDistance = s.maxDistanceSound;
            s.source.rolloffMode = s.audioRolloffMode;
        }
    }

    public void PlayOneSound(string name)
    {
        // here we get the Sound from our array with the name passed in the methods parameters
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Unable to play sound " + name);
            return;
        }
        s.source.Play(); // play the sound
    }
    
    public void StopOneSound(string name)
    {
        // here we get the Sound from our array with the name passed in the methods parameters
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Unable to play sound " + name);
            return;
        }
        s.source.Stop(); // stop the sound
    }

    public void PlayFootStepsArray()
    {
        if (shouldPlayEnemyArraySounds == false) 
        {
            shouldPlayEnemyArraySounds = true;
            // pick a random song from our playlist
            currentPlayingArrayIndex = UnityEngine.Random.Range(0, enemyArraySounds.Length - 1);
            enemyArraySounds[currentPlayingArrayIndex].source.volume = enemyArraySounds[0].volume * mvol; // set the volume
            enemyArraySounds[currentPlayingArrayIndex].source.Play(); // play it
            StopMusic();
        }
    }

    private void FootStepsIndex()
    {
        // if we are playing a track from the playlist && it has stopped playing
        if (currentPlayingArrayIndex != 999 && !enemyArraySounds[currentPlayingArrayIndex].source.isPlaying)
        {
            currentPlayingArrayIndex++; // set next index
            if (currentPlayingArrayIndex >= enemyArraySounds.Length)
            { //have we went too high
                currentPlayingArrayIndex = 0; // reset list when max reached
            }
            enemyArraySounds[currentPlayingArrayIndex].source.Play(); // play that funky music
        }

    }

    
    public void StopMusic()
    {
        if (shouldPlayEnemyArraySounds == true) 
        {
            shouldPlayEnemyArraySounds = false;
            
            currentPlayingArrayIndex = 999; // reset playlist counter
            
        }
    }



    // if the music volume change update all the audio sources
    public void musicVolumeChanged()
    {
        foreach (Sound m in enemyArraySounds)
        {
            mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            m.source.volume = enemyArraySounds[0].volume * mvol;
        }
    }

    //if the effects volume changed update the audio sources
    public void effectVolumeChanged() 
    {
        evol = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);
        foreach (Sound s in sounds) 
        {
            s.source.volume = s.volume * evol;
        }
        sounds[0].source.Play(); // play an effect so user can her effect volume
    }

}
