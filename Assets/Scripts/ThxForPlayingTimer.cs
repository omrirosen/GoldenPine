using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThxForPlayingTimer : MonoBehaviour
{
    [SerializeField] private float fadeTextTimer = 10f;
    private float timePassed = 0f;
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        timePassed = 0f;
    }
    

    private void Update()
    { 
        timePassed += Time.deltaTime;
        
      if (timePassed >= fadeTextTimer)
      {
          animator.SetBool("FadeText", true);
      }
    }
    
    
}
