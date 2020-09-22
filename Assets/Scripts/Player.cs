using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("Player Config")]
   [SerializeField] private float playerSpeed;
   [SerializeField] private float jumpForce;
   [SerializeField] private float fallMultiplier;
   [SerializeField] private float lowJumpMultiplier;
   [SerializeField] private float wallSlidingSpeed;
   [SerializeField] private float xWallForce;
   [SerializeField] private float yWallForce;
   [SerializeField] private float wallJumpTime;
   private float moveInput;
   
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
   private bool charIsFalling = false;
   
   // Component Caches
   private Rigidbody2D rb;
   private Animator anim;



   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
   }

   private void Update()
   {
      HorizontalMovement();
      PlayerJump();
      WallMovement();

      if (moveInput != 0)
      {
         anim.SetBool("isRunning", true);
      }
      else
      {
         anim.SetBool("isRunning", false);
      }

      if (isGrounded == true)
      {
         anim.SetBool("isJumping", false);
         anim.SetBool("isFalling", false);

      }
      else
      {
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


   private void HorizontalMovement()
   {
      moveInput = Input.GetAxisRaw("Horizontal");  //raw meaning snappy movement, remove raw for fluidity
      rb.velocity = new Vector2(moveInput * playerSpeed, rb.velocity.y);
      if (moveInput > 0 && facingRight == false)
      {
         FlipSprite();
      }
      else if (moveInput < 0 && facingRight == true)
      {
         FlipSprite();
      }
   }

   private void PlayerJump()
   {
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
      
      if (Input.GetButtonDown("Jump") && isGrounded == true)
      {
         anim.SetTrigger("takeOff");
         rb.velocity = Vector2.up * jumpForce;
      }

      if (rb.velocity.y < 0)
      {
         rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
      }
      else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
      {
         rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
      }
   }

   private void WallMovement()
   {
      isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);
      if (isTouchingFront == true && isGrounded == false && moveInput != 0)
      {
         wallSliding = true;
      }
      else
      {
         wallSliding = false;
      }

      if (wallSliding == true)
      {
         rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed,float.MaxValue));
      }

      if (Input.GetButtonDown("Jump") && wallSliding == true)
      {
         wallJumping = true;
         Invoke("SetWallJumpingToFalse", wallJumpTime);
      }

      if (wallJumping == true)
      {
         rb.velocity = new Vector2(xWallForce * -moveInput, yWallForce);
      }
   }
   
   void FlipSprite()
   {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
      facingRight = !facingRight;
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
}
