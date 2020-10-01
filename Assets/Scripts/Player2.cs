﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
   [Header("Movement Config")]
   [SerializeField] private float playerSpeed;
   [SerializeField] private float airMoveSpeed;
   private float xMoveInput;
   private bool isMoving;
   private bool facingRight = true;
   
   [Header("Jump Config")]
   [SerializeField] private float jumpForce;
   [SerializeField] private float jumpTime;
   private float jumpTimeCounter;
   public bool isGrounded;
   private bool isFalling = false;
   public bool isJumping;
   private bool reachedPeakJump;

   [Header("Wall Slide Config")]
   [SerializeField] private float wallSlideSpeed;
   private bool isTouchingWall;
   private bool isWallSliding;
   
   [Header("Wall Jump Config")]
   [SerializeField] private Vector2 wallJumpAngle;
   [SerializeField] private float wallJumpForce;
   private float wallJumpDirection = -1;
   

   [Header("DashConfig")] 
   [SerializeField] private float dashSpeed;
   [SerializeField] private float startDashTime;
   private bool isDashing;
   private FadingGhost fadingGhost;
   private float dashTime;
   private int direction;

   [Header("Collision Check")] 
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private Transform groundCheckPos;
   [SerializeField] private Vector2 groundCheckSize;
   [SerializeField] private Transform wallCheckPos;
   [SerializeField] private Vector2 wallCheckSize;
   
   

   // Component Caches
   private Rigidbody2D rb;
   private Animator anim;

   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      fadingGhost = FindObjectOfType<FadingGhost>();
   }

   private void Start()
   {
      dashTime = startDashTime;
      wallJumpAngle.Normalize();
   }

   private void Update()
   {
      Inputs();
      AnimationSetup();
      PlayerJump();
      WallJump();
   }

   private void FixedUpdate()
   {
      CollisionChecks();
      PlayerMovement();
      HandleDash();
      WallSlide();
   }

   private void Inputs()
   
   {
      //Horizontal Inputs
      xMoveInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw meaning snappy movement, remove raw for fluidity
      
      //Dash Inputs
      if (Input.GetKeyDown(KeyCode.LeftShift))
      {
         isDashing = true;
      }
   }

   private void CollisionChecks()
   {
      isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
      isTouchingWall = Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, groundLayer);
   }


   private void PlayerMovement()
   {
      // for animation
      if (xMoveInput !=0)
      {
         isMoving = true;
      }
      else
      {
         isMoving = false;
      }
      
      // for movement
      if (isMoving)
      {
         rb.velocity = new Vector2(xMoveInput * playerSpeed, rb.velocity.y);
      }
     
      else if (!isGrounded &&(!isWallSliding || !isTouchingWall) && xMoveInput != 0)
      {
         rb.AddForce(new Vector2(airMoveSpeed * xMoveInput,0));
         if (Mathf.Abs(rb.velocity.x) > playerSpeed)
         {
            rb.velocity = new Vector2(xMoveInput * playerSpeed, rb.velocity.y);
         }
      }
      else
      {
         rb.velocity = new Vector2(0,rb.velocity.y);
      }
      
      
      // for flipping
      if (xMoveInput < 0 && facingRight )
      {
         FlipSprite();
      }
      else if (xMoveInput > 0 && !facingRight)
      {
         FlipSprite();
      }
   }

   private void PlayerJump()
   {
      
      if (Input.GetButtonDown("Jump") && isGrounded)
      {
         anim.SetTrigger("takeOff");
         isJumping = true;
         jumpTimeCounter = jumpTime;
         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }

      if (Input.GetButton("Jump") && isJumping)
      {
         if (jumpTimeCounter > 0)
         {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
         }
         else
         {
            isJumping = false;
         }
         
      }

      if (Input.GetButtonUp("Jump"))
      {
         isJumping = false;
      }
      
   }
   
   
   private void HandleDash()
   {
      if (direction == 0)
      {
         if (isDashing)
         {
            isDashing = true;
            if (!facingRight) // left 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashTime);
               direction = 1;
            }
            else if (facingRight) // right 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashTime);
               direction = 2;
            }
         }
      }
      else
      {
         if (dashTime <= 0)
         {
            direction = 0;
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;
            Invoke("SetIsDashingToFalse",0.2f);
         }
         else
         {
            fadingGhost.createGhost = true;
            Invoke("SetCreateGhostToFalse",dashTime);
            dashTime -= Time.deltaTime;
            if (direction == 1)
            {
               rb.velocity = Vector2.left * dashSpeed;
               Invoke("SetIsDashingToFalse",0.2f);
               
            }
            else if (direction == 2)
            {
               rb.velocity = Vector2.right * dashSpeed;
               Invoke("SetIsDashingToFalse",0.2f);
            }
         }
      }
      
   }

   void WallSlide()
   {
      if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
      {
         anim.SetTrigger("touchedWall");
         isWallSliding = true;
      }
      else
      {
         isWallSliding = false;
      }

      if (isWallSliding)
      {
         rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
      }
   }

   void WallJump()
   {
      if (Input.GetButtonDown("Jump"))
      {
         if ((isWallSliding) && !isGrounded)
         {
            rb.AddForce(new Vector2(wallJumpForce * wallJumpAngle.x * wallJumpDirection, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            FlipSprite();
         
         }
      }
      
   }

   void FlipSprite()
   {
      if (!isWallSliding)
      {
         wallJumpDirection *= -1;
         facingRight = !facingRight;
         transform.Rotate(0, 180, 0);
      }
   }

   private void SetCreateGhostToFalse()
   {
      fadingGhost.createGhost = false;
   }

   void SetReachedPeakToFlase()
   {
      reachedPeakJump = false;
   }

   void SetIsDashingToFalse()
   {
      isDashing = false;
   }

   private void AnimationSetup()
   {
      anim.SetBool("isMoving", isMoving);
      anim.SetBool("isGrounded", isGrounded);
      anim.SetBool("isTouchingWall", isTouchingWall);
      anim.SetBool("isDashing", isDashing);
      anim.SetBool("atPeakJump", reachedPeakJump);

      if (rb.velocity.y < 0 && reachedPeakJump == false)
      {
         reachedPeakJump = true;
         Invoke("SetReachedPeakToFlase", 0.05f);
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
      Gizmos.color = Color.red;
      Gizmos.DrawCube(wallCheckPos.position, wallCheckSize);
   }
}