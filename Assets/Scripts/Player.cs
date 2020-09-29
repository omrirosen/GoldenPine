using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("Movement Config")]
   [SerializeField] private float playerSpeed;
   private float moveInput;
   
   [Header("Jump Config")]
   [SerializeField] private float jumpForce;
   [SerializeField] private float jumpTime;
   private float jumpTimeCounter;
   
   [Header("Wall Config")]
   [SerializeField] private float wallSlidingSpeed;
   [SerializeField] private float xWallForce;
   [SerializeField] private float yWallForce;
   [SerializeField] private float wallJumpTime;

   [Header("DashConfig")] 
   [SerializeField] private float dashSpeed;
   [SerializeField] private float startDashTime;
   private FadingGhost fadingGhost;
   private float dashTime;
   private int direction;
   
   
   [Header("Collision Checks")]
   [SerializeField] private float checkRadius;
   public Transform groundCheck;
   public Transform frontCheck;
   public LayerMask whatIsGround;
   
   //Bool's
   private bool facingRight = true;
   private bool isGrounded;
   private bool isTouchingFront;
   private bool wallSliding;
   private bool wallJumping;
   private bool reachedPeakJump = false;
   private bool isFalling = false;
   private bool isJumping;
   
   // Component Caches
   private Rigidbody2D rb;
   private Animator anim;



   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      fadingGhost = FindObjectOfType<FadingGhost>();
      dashTime = startDashTime;
   }

   private void Update()
   {
      PlayerControls();
      AnimationSetup();
      HandleDash();
   }

   private void PlayerControls()
   {
      HorizontalMovement();
      PlayerJump();
      WallMovement();
   }
   private void HorizontalMovement()
   {
      moveInput = Input.GetAxisRaw("Horizontal");  //GetAxisRaw meaning snappy movement, remove raw for fluidity
      rb.velocity = new Vector2(moveInput * playerSpeed, rb.velocity.y);
      if (moveInput > 0 && facingRight == false)
      {
         FlipSprite();
      }
      else if (moveInput < 0 && facingRight)
      {
         FlipSprite();
      }
   }

   private void PlayerJump()
   {
     
   isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
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

   private void WallMovement()
   {
      isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);
      if (isTouchingFront && isGrounded == false && moveInput != 0)
      {
         wallSliding = true;
      }
      else
      {
         wallSliding = false;
      }

      if (wallSliding)
      {
         rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed,float.MaxValue));
      }

      if (Input.GetButtonDown("Jump") && wallSliding)
      {
         wallJumping = true;
         Invoke("SetWallJumpingToFalse", wallJumpTime);
      }

      if (wallJumping)
      {
         rb.velocity = new Vector2(xWallForce * -moveInput, yWallForce);
      }
   }

   private void HandleDash()
   {
      if (direction == 0)
      {
         if (Input.GetKeyDown(KeyCode.LeftShift))
         {
            anim.SetBool("isDashing", true);
            if (!facingRight) // left 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashSpeed);
               direction = 1;
            }
            else if (facingRight) // right 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashSpeed);
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

   void FlipSprite()
   {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
      facingRight = !facingRight;
   }

   private void SetCreateGhostToFalse()
   {
      fadingGhost.createGhost = false;
   }

   void SetWallJumpingToFalse()
   {
      wallJumping = false;
   }

   void SetReachedPeakToFlase()
   {
      reachedPeakJump = false;
      anim.SetBool("atPeak", false);
   }

   void SetIsDashingToFalse()
   {
      anim.SetBool("isDashing", false);
   }

   private void AnimationSetup()
   {
      if (moveInput != 0)
      {
         anim.SetBool("isRunning", true);
      }
      else
      {
         anim.SetBool("isRunning", false);
      }

      if (facingRight)
      {
         anim.SetBool("facingRight", true);
      }
      else
      {
         anim.SetBool("facingRight", false);
      }

      if (isGrounded)
      {
         anim.SetBool("isGrounded", true);
         anim.SetBool("isJumping", false);
         anim.SetBool("isFalling", false);

      }
      else
      {
         anim.SetBool("isGrounded", false);
         anim.SetBool("isJumping", true);
         anim.SetBool("isFalling", true);
      }
      

      if (rb.velocity.y < 0 && this.reachedPeakJump == false)
      {
         this.reachedPeakJump = true;
         anim.SetBool("atPeak", true);
         Invoke("SetReachedPeakToFlase", 0.05f);
      }
   }
}
