using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    public Sound[] sounds;      // store all our sounds
    public Sound[] footstepsArray;    // store all our music
    public Sound[] dashArray;
    public Sound[] zButtonArray;

    private int currentPlayingFootStepsIndex = 999; // set high to signify no song playing
    private int currentPlayingDashIndex = 999;
    private int currentPlayingZButton = 999;
    // a play music flag so we can stop playing music during cutscenes etc
    private bool shouldPlayMusic = false;
    private bool shouldPlayDashArray = false;
    private bool shouldPlayZButtonArray = false;
    public static SoundManager instance; // will hold a reference to the first AudioManager created

    private float mvol; // Global music volume
    private float evol; // Global effects volume

    private void Start() 
    {
        
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
        createAudioSources(dashArray, mvol);
        createAudioSources(zButtonArray, mvol);
    }
    void Update()
    {
        FootStepsIndex();
        DashIndex();
        ZButtonIndex();
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

    public void PlayFootStepsArray()
    {
        if (shouldPlayMusic == false) 
        {
            shouldPlayMusic = true;
            // pick a random song from our playlist
            currentPlayingFootStepsIndex = UnityEngine.Random.Range(0, footstepsArray.Length - 1);
            footstepsArray[currentPlayingFootStepsIndex].source.volume = footstepsArray[0].volume * mvol; // set the volume
            footstepsArray[currentPlayingFootStepsIndex].source.Play(); // play it
            StopMusic();
        }
    }

    private void FootStepsIndex()
    {
        // if we are playing a track from the playlist && it has stopped playing
        if (currentPlayingFootStepsIndex != 999 && !footstepsArray[currentPlayingFootStepsIndex].source.isPlaying)
        {
            currentPlayingFootStepsIndex++; // set next index
            if (currentPlayingFootStepsIndex >= footstepsArray.Length)
            { //have we went too high
                currentPlayingFootStepsIndex = 0; // reset list when max reached
            }
            footstepsArray[currentPlayingFootStepsIndex].source.Play(); // play that funky music
        }

    }

    public void PlayDashArray()
    {
        if (shouldPlayDashArray == false)
        {
            shouldPlayDashArray = true;
            // pick a random song from our playlist
            currentPlayingDashIndex = UnityEngine.Random.Range(0, dashArray.Length - 1);
            dashArray[currentPlayingDashIndex].source.volume = dashArray[0].volume * mvol; // set the volume
            dashArray[currentPlayingDashIndex].source.Play(); // play it
            StopMusic();
        }
    }
    private void DashIndex()
    {
        // if we are playing a track from the playlist && it has stopped playing
        if (currentPlayingDashIndex != 999 && !dashArray[currentPlayingDashIndex].source.isPlaying)
        {
            currentPlayingDashIndex++; // set next index
            if (currentPlayingDashIndex >= dashArray.Length)
            { //have we went too high
                currentPlayingDashIndex = 0; // reset list when max reached
            }
            dashArray[currentPlayingDashIndex].source.Play(); // play that funky music
        }

    }



     public void PlayzButtonArray()
     {
         if (shouldPlayZButtonArray == false)
         {
            shouldPlayZButtonArray = true;
            // pick a random song from our playlist
            currentPlayingZButton = UnityEngine.Random.Range(0, zButtonArray.Length - 1);
             zButtonArray[currentPlayingZButton].source.volume = zButtonArray[0].volume * mvol; // set the volume
             zButtonArray[currentPlayingZButton].source.Play(); // play it
             StopMusic();
         }
     }

    private void ZButtonIndex()
    {
        // if we are playing a track from the playlist && it has stopped playing
        if (currentPlayingZButton != 999 && !zButtonArray[currentPlayingZButton].source.isPlaying)
        {
            currentPlayingZButton++; // set next index
            if (currentPlayingZButton >= zButtonArray.Length)
            { //have we went too high
                currentPlayingZButton = 0; // reset list when max reached
            }
            zButtonArray[currentPlayingZButton].source.Play(); // play that funky music
        }
    }
    // stop music
    public void StopMusic()
    {
        if (shouldPlayMusic == true) 
        {
            shouldPlayMusic = false;
            
            currentPlayingFootStepsIndex = 999; // reset playlist counter
            
        }

        if(shouldPlayDashArray == true)
        {
            shouldPlayDashArray = false;
            currentPlayingDashIndex = 999;
        }
        if(shouldPlayZButtonArray == true)
        {
            shouldPlayZButtonArray = false;
            currentPlayingZButton = 999;
        }
    }



    // get the song name
  /*  public String getSongName()
    {
        return footstepsArray[currentPlayingFootStepsIndex].name;
    }*/

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