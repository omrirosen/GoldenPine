using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThxForPlayingTimer : MonoBehaviour
{
    [SerializeField]private GameObject endText;
    [SerializeField]private int txtShowScore =5;
    private ScoreManager scoreManager;
   
    
    public static ThxForPlayingTimer instance;
    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
  

    private void Update()
    {
      if(scoreManager.currentScore >= txtShowScore)
      {
            endText.SetActive(true);
      }
    }
}
