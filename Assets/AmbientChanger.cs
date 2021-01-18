using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChanger : MonoBehaviour
{
   public static AmbientChanger instance;
   private SoundManager soundManager;
   

   private void Awake()
   {
      soundManager = FindObjectOfType<SoundManager>();
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
   }
}
