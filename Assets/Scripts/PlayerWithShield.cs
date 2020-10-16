using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithShield : MonoBehaviour
{
   [Header("Movement Config")]
   [SerializeField] private float playerSpeed = 5f;
   [SerializeField] private float airMoveSpeed = 5f;
   private float xMoveInput;
   public bool isMoving;
   private bool facingRight = true;
   
   [Header("Jump Config")]
   [SerializeField] private float jumpForce = 8f;
   [SerializeField] private float jumpTime = .35f;
   private float jumpTimeCounter;
   private bool isFalling = false;
   public bool isJumping;
   public bool reachedPeakJump;

   [Header("Wall Slide Config")]
   [SerializeField] private float wallSlideSpeed = .5f;
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

   [Header("ShieldBubble Config")] 
   [SerializeField] private GameObject shieldBubble;
   [SerializeField] private SpriteRenderer shieldBubbleSR;
   [SerializeField] private CircleCollider2D shieldBubbleCC2D;
    public bool isShielding;
    [SerializeField] private PlayerStats PS;

    // Component Caches
    private Rigidbody2D rb;
   private Animator anim;
  
   
   private CollisionCheck collisionCheck;

   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      collisionCheck = GetComponent<CollisionCheck>();
      fadingGhost = FindObjectOfType<FadingGhost>();
      shieldBubbleSR = shieldBubble.GetComponent<SpriteRenderer>();
      shieldBubbleCC2D = shieldBubble.GetComponent<CircleCollider2D>();


   }

   private void Start()
   {
      dashTime = startDashTime;
      wallJumpAngle.Normalize();
   }

   private void Update()
   {
      Inputs();
      PlayerJump();
      PlayerMovement();
      WallJump();
      HandleDash();
      HandleShield();
      WallSlide();
      AnimationSetup();
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
      
      //Shield Inputs
      if (Input.GetKey(KeyCode.X))
      {
         isShielding = true;
      }
      else
      {
         Invoke("SetIsShieldingToFalse",1f);
      }
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
     
      else if (!collisionCheck.onGround &&(!isWallSliding || !collisionCheck.onWall) && xMoveInput != 0)
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
      
      if (Input.GetButtonDown("Jump") && collisionCheck.onGround)
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

   void HandleShield()
   {
      if (isShielding)
      {
         shieldBubbleSR.enabled = true;
         shieldBubbleCC2D.enabled = true;
      }
      else
      {
         shieldBubbleSR.enabled = false;
         shieldBubbleCC2D.enabled = false;
      }
   }
   

   void WallSlide()
   {
        
      if (collisionCheck.onWall && !collisionCheck.onGround && rb.velocity.y < 0)
      {
         anim.SetTrigger("touchedWall");
         isWallSliding = true;
            anim.SetBool("StartSlide", true);
        }
      else
      {
         isWallSliding = false;
            anim.SetBool("StartSlide", false);
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
         if ((isWallSliding) && !collisionCheck.onGround)
         {
            rb.AddForce(new Vector2(wallJumpForce * wallJumpAngle.x * wallJumpDirection, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            FlipSprite();
         
         }
      }
      
   }

   void FlipSprite()
   {
      if (!collisionCheck.onWall)
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

   void SetIsShieldingToFalse()
   {
      isShielding = false;
   }
   

   private void AnimationSetup()
   {
      anim.SetBool("isMoving", isMoving);
      anim.SetBool("isGrounded", collisionCheck.onGround);
      anim.SetBool("isTouchingWall", collisionCheck.onWall);
      anim.SetBool("isDashing", isDashing);
      anim.SetBool("atPeakJump", reachedPeakJump);

      if (rb.velocity.y < 0 && reachedPeakJump == false)
      {
         reachedPeakJump = true;
         Invoke("SetReachedPeakToFlase", 0.05f);
      }
   }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("bla");
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hitEnemy");
            if (PS.DashAttacked == true)
            {
                collision.gameObject.GetComponent<Unihog1Controller>().killme();
            }
        }
    }

}
