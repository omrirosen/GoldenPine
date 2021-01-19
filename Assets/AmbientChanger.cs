using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
   public static AmbientChanger instance;
   private SoundManager soundManager;
   private PlayerSoundManager playerSoundManager;

   private void Awake()
   {
      soundManager = FindObjectOfType<SoundManager>();
      playerSoundManager = FindObjectOfType<PlayerSoundManager>();
      if (instance == null)
      {     
         instance = this;        
      }
      
      else
      {
         Destroy(gameObject);    
         return;                
      }
      DontDestroyOnLoad(gameObject); 
   }

   private void Start()
   {
      soundManager.StopOneSound("Ambient Forest Wind");
      soundManager.PlayOneSound("Ambient Cave Wind (No Owl)");
      soundManager.StopOneSound("Ambient Music");
      playerSoundManager.inCave = true;
   }
}
