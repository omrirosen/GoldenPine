using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    public Sound[] sounds;      // store all our sounds
    public Sound[] footstepsArray;    // store all our music

    private int currentPlayingIndex = 999; // set high to signify no song playing

    // a play music flag so we can stop playing music during cutscenes etc
    private bool shouldPlayMusic = false; 

    public static SoundManager instance; // will hold a reference to the first AudioManager created

    private float mvol; // Global music volume
    private float evol; // Global effects volume

    private void Start() 
    {
        //start the music
        PlayArraySound();
        PlayOneSound("Ambient Forest");
        PlayOneSound("Ambient Wind");
    }


    private void Awake()
    {

        if (instance == null)
        {     // if the instance var is null this is first AudioManager
            instance = this;        //save this AudioManager in instance 
        } else
        {
            Destroy(gameObject);    // this isnt the first so destroy it
            return;                 // since this isn't the first return so no other code is run
        }

        DontDestroyOnLoad(gameObject); // do not destroy me when a new scene loads

        // get preferences
        mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        evol = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);

        createAudioSources(sounds, evol);     // create sources for effects
        createAudioSources(footstepsArray, mvol);   // create sources for music

    }

    // create sources
    private void createAudioSources(Sound[] sounds, float volume)
    {
        foreach (Sound s in sounds) {   // loop through each music/effect
            s.source = gameObject.AddComponent<AudioSource>(); // create anew audio source(where the sound splays from in the world)
            s.source.clip = s.clip;     // the actual music/effect clip
            s.source.volume = s.volume * volume; // set volume based on parameter
            s.source.pitch = s.pitch;   // set the pitch
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.loop = s.loop;     // should it loop
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

    public void PlayArraySound()
    {
        if (shouldPlayMusic == false) 
        {
            shouldPlayMusic = true;
            // pick a random song from our playlist
            currentPlayingIndex = UnityEngine.Random.Range(0, footstepsArray.Length - 1);
            footstepsArray[currentPlayingIndex].source.volume = footstepsArray[0].volume * mvol; // set the volume
            footstepsArray[currentPlayingIndex].source.Play(); // play it
            StopMusic();
        }
    }

    // stop music
    public void StopMusic()
    {
        if (shouldPlayMusic == true) 
        {
            shouldPlayMusic = false;
            currentPlayingIndex = 999; // reset playlist counter
        }
    }

    void Update()
    {
        // if we are playing a track from the playlist && it has stopped playing
        if (currentPlayingIndex != 999 && !footstepsArray[currentPlayingIndex].source.isPlaying) 
        {
            currentPlayingIndex++; // set next index
            if (currentPlayingIndex >= footstepsArray.Length)
            { //have we went too high
                currentPlayingIndex = 0; // reset list when max reached
            }
            footstepsArray[currentPlayingIndex].source.Play(); // play that funky music
        }
    }

    // get the song name
    public String getSongName()
    {
        return footstepsArray[currentPlayingIndex].name;
    }

    // if the music volume change update all the audio sources
    public void musicVolumeChanged()
    {
        foreach (Sound m in footstepsArray)
        {
            mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            m.source.volume = footstepsArray[0].volume * mvol;
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