using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class ShieldBubble : MonoBehaviour
{
    private PlayerWithShield playerScript;
    private CollisionCheck collisionCheck;
    private Animator shieldAnimator;
    public bool isShielding;
    public bool isGrounded;
    public bool isMoving;
    public bool reachedPeackJump;
    private void Awake()
    {
        playerScript = GetComponentInParent<PlayerWithShield>();
        collisionCheck = GetComponentInParent<CollisionCheck>();
        shieldAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimationSetup();
        isShielding = playerScript.isShielding;
        isGrounded = collisionCheck.onGround;
        isMoving = playerScript.isMoving;
        reachedPeackJump = playerScript.reachedPeakJump;


    }
    
    private void AnimationSetup()
    {
        shieldAnimator.SetBool("isGrounded", isGrounded);
        shieldAnimator.SetBool("isShielding", isShielding);
        shieldAnimator.SetBool("isMoving", isMoving);
        //shieldAnimator.SetBool("isFalling", reachedPeackJump);
        if (Input.GetKeyDown(KeyCode.X))
        {
            shieldAnimator.SetTrigger("startedShielding");
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            //AudioManager.PlaySound(Sounds.ShieldPop);
            if (isGrounded)
            {
                shieldAnimator.Play("Shield Ground Pop");
            }
            else
            {
                shieldAnimator.Play("Shield Ground Pop");
            }
            
        }
    }

    public void HitShield()
    {
        
        if (isGrounded)
        {
            shieldAnimator.Play("Shield Ground Pop");
        }
        if(!isGrounded)
        {
            shieldAnimator.Play("Shield Ground Pop");
        }
        //AudioManager.PlaySound(Sounds.ShieldPop);
    }
    
}
