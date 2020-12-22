using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadUniHogTut : MonoBehaviour
{
    public int numOfHitTaken = 0;
    private Animator uniAnimator;
    private BoxCollider2D myBoxCollider;

    private void Start()
    {
        uniAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleDeathAnimation();
    }
    
    void HandleDeathAnimation()
    {
        if (numOfHitTaken >= 999)
        {
            myBoxCollider.enabled = false;
            uniAnimator.SetBool("isDead", true);
            Invoke("DestroyHog", 1);
        }
        
    }

    void DestroyHog()
    {
        Destroy(this.gameObject);
    }
}
