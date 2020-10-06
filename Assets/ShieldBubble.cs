using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubble : MonoBehaviour
{
    private PlayerWithShield playerScript;
    private CollisionCheck collisionCheck;
    private Animator shieldAnimator;
    public bool isShielding;
    public bool isGrounded;
    public bool isMoving;

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
    }
    
    private void AnimationSetup()
    {
        shieldAnimator.SetBool("isGrounded", isGrounded);
        shieldAnimator.SetBool("isShielding", isShielding);
        shieldAnimator.SetBool("isMoving", isMoving);
        if (Input.GetKeyDown(KeyCode.X))
        {
            shieldAnimator.SetTrigger("startedShielding");
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            shieldAnimator.SetTrigger("stoppedShielding");
        }
    }
    
}
