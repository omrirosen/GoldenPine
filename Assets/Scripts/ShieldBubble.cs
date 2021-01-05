using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class ShieldBubble : MonoBehaviour
{
    private PlayerWithShield playerScript;
    private CollisionCheck collisionCheck;
    public PlayerStats playerStats;
    private Animator shieldAnimator;
    public bool isShielding;
    public bool isGrounded;
    public bool isMoving;
    public bool reachedPeackJump;
    private SoundManager soundManager;
    
    private void Awake()
    {
        playerScript = GetComponentInParent<PlayerWithShield>();
        collisionCheck = GetComponentInParent<CollisionCheck>();
        shieldAnimator = GetComponent<Animator>();
        soundManager = FindObjectOfType<SoundManager>();
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
            soundManager.PlayOneSound("Shield Up");
            //AudioManager.PlaySound(Sounds.ShieldUp);
        }
        if (playerScript.isShielding)
        {
            if (Input.GetKeyUp(KeyCode.X) || Input.GetKey(KeyCode.LeftShift))
            {
                 //AudioManager.PlaySound(Sounds.ShieldPop);
                 if (isGrounded)
                {
                    shieldAnimator.Play("Shield Ground Pop");
                }
                else
                {
                    shieldAnimator.Play("Shield Air Pop");
                }

            }
        }
    }

    public void HitShield()
    {
        //AudioManager.PlaySound(Sounds.ShieldPop);

        if (isGrounded)
        {
            shieldAnimator.Play("Shield Ground Pop");
        }
        if(!isGrounded)
        {
            shieldAnimator.Play("Shield Air Pop");
        }
       
    }

    public void ConnectToTakeDMG(int dmg,Vector3 dir )
    {
        if(playerStats!=null)
        {
            playerStats.TakeDmg(dmg, dir);
        }
        else
        {
            print("faild");
        }
   
       
       
    }
    public bool ConnectToShidStatus()
    {
        bool shildON = playerStats.shieldOn;
        bool ParryWindow = playerStats.ParryWindow;
        if (shildON || ParryWindow)
        {
            return true;
        }
        else return false;
    }
    
}
