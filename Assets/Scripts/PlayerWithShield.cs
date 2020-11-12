using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JSAM;
using UnityEngine;

public class PlayerWithShield : MonoBehaviour
{
   [Header("Movement Config")]
   [SerializeField] private float playerSpeed = 3f;
   [SerializeField] private float airMoveSpeed = 3f;
   [SerializeField] private float originalPlayerSpeed = 3f;
   [SerializeField] private float shieldingPlayerSpeed = 1.5f;
   [SerializeField] private float originalJumpForce = 5f;
   [SerializeField] private float shieldingJumpForce = 2.5f;
   private float xMoveInput;
   public bool isMoving;
   private bool facingRight = true;
   
   [Header("Jump Config")]
   [SerializeField] private float jumpForce = 5f;
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
   [SerializeField] private float dashSpeed = 10f;
   [SerializeField] private float startDashTime;
   [SerializeField] int dmg;
   private float HoldDashStartTime;
   private float HoldDashTime;
   private float DashSpeedMax = 20f;
   public bool isDashing;
   private bool DashCooldown = false;
   private FadingGhost fadingGhost;
   private float dashTime;
   private int direction;
    float HoldDownTime;
   [Header("ShieldBubble Config")] 
   [SerializeField] private GameObject shieldBubble;
   [SerializeField] private SpriteRenderer shieldBubbleSR;
   [SerializeField] private CircleCollider2D shieldBubbleCC2D;
    public bool isShielding;
    [SerializeField] private PlayerStats PS;
    
    // Component Caches
   private Rigidbody2D rb;
   private Animator anim;
   public BuddyController Buddy;
   private CollisionCheck collisionCheck;
   [SerializeField] private CinemachineImpulseSource pulseSource;
   public bool DashAttack = false;
    [SerializeField] GameObject ChargAnim;
    bool IsDead = false;
    bool CanShield = true;
    GameObject Player;
    bool DashCharging = false;
    GameManager GM;
    bool PirceingDash = false;
    private void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      collisionCheck = GetComponent<CollisionCheck>();
      fadingGhost = FindObjectOfType<FadingGhost>();
      shieldBubbleSR = shieldBubble.GetComponent<SpriteRenderer>();
      shieldBubbleCC2D = shieldBubble.GetComponent<CircleCollider2D>();
        Player = this.gameObject;
        GM = FindObjectOfType<GameManager>();
    }

   private void Start()
   {
      dashTime = startDashTime;
      wallJumpAngle.Normalize();
   }

   private void Update()
   {
      if(IsDead == false) 
      {
          Inputs();
          PlayerJump();
          PlayerMovement();
          //WallJump();
        //  HandleDash();
          HandleShield();
          WallSlide();
          AnimationSetup();
            print("calc dashTime" + CalculateDashTime(HoldDownTime));
      }
      else
      {
            Invoke("OnDeath", 2f);
      }

   }

   private void FixedUpdate()
   {
      HandleDash();
   }

   private void OnDeath()
    {
        GM.ResetScene();
    }

   private void Inputs()
   
   {
      //Horizontal Inputs
      xMoveInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw meaning snappy movement, remove raw for fluidity
                                                  //Dash Inputs
      if (Input.GetKeyDown(KeyCode.LeftShift) && DashCooldown == false)
      {

            HoldDashStartTime = Time.time;
            DashCharging = true;
            Invoke("SetDashChagingFalse", 0.5f);
           
      }

      if (Input.GetKey(KeyCode.LeftShift))
      {
            HoldDownTime = Time.time - HoldDashStartTime;
            if (CalculateDashTime(HoldDownTime) > 1f && PS.DashStock >= 1)
            {
                isDashing = false;
                PirceingDash = true;
            }
            if (CalculateDashTime(HoldDownTime) < 1f && PS.DashStock >= 1f || PS.DashStock < 1)
            {
                PirceingDash = false;

            }

      }
     
      if (Input.GetKeyUp(KeyCode.LeftShift) && DashCooldown == false)
      {
            anim.SetBool("IsWhite", false);
            
            //dashSpeed = dashSpeed + CalculateDashTime(HoldDownTime);
            if(PirceingDash == true)
            {
                dashSpeed = 20;
                PS.DashAttackOn = true;
                isDashing = false;
            }

            if (PirceingDash == false)
            {
                isDashing = true;
                dashSpeed = 10;
            }
            /*if(dashSpeed > 20) dashSpeed = 20;
            if (dashSpeed < 12 || PS.DashStock < 1)
            {
               dashSpeed = 10;
               isDashing = true;
            }
            if(dashSpeed >= 12)
            {
                
               PS.DashAttackOn = true;
                Invoke("DashingToFalse", 0.1f);
            }*/
            
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            Buddy.Dash();
            GeneratPulse();
            DashCharging = false;
            Invoke("BackToOGDashSpeed", 0.2f);
            DashCooldown = true;
            Invoke("ResetDashCoolDown", 0.5f);
            CalculateDashTime(0);
      }
        
        
       
      //Shield Inputs
      
      if (Input.GetKey(KeyCode.X) && CanShield == true)
      {
         isShielding = true;
      }
      
      else
      {
         Invoke("SetIsShieldingToFalse",1f);
      }
   }
    private float CalculateDashTime(float HoldTime)
    {
        float MaxSpeedHoldTime = 2f;
        float NormalizedHoldTime = Mathf.Clamp(HoldTime ,0, MaxSpeedHoldTime);
        //float force = NormalizedHoldTime * DashSpeedMax;
        return NormalizedHoldTime;
    }
    private void DashingToFalse()
    {
        isDashing = false;
    }

    private void BackToOGDashSpeed()
    {
        dashSpeed = 10f;
        PS.DashAttackOn = false;
        isDashing = false;
        PirceingDash = false;
    }
    public void GeneratPulse()
    {
        pulseSource.GenerateImpulse();
    }
    private void ResetDashCoolDown()
    {
        DashCooldown = false;
    }
    
    private void SetDashChagingFalse()
    {
        if (PS.DashStock >= 1 && DashCharging == true)
        {
            ChargAnim.SetActive(true);
            Invoke("SetWhite", 0.5f);
            DashCharging = false;
        }
    }
   private void SetWhite()
   {
        anim.SetBool("TransitionToWhite", true);
        anim.SetBool("IsWhite", true);
        Invoke("EndTransitionToWhite", 0.2f);
   }

    private void EndTransitionToWhite()
    {
        anim.SetBool("TransitionToWhite", false);
    }

   private void PlayerMovement()
   {
      // for animation
      if (xMoveInput !=0)
      {
         isMoving = true;
            Buddy.Run();
           // print("Running");
      }
      else
      {
         isMoving = false;
            Buddy.StopRun();
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
         //JSAM.AudioManager.PlaySound(Sounds.Jump); 
         anim.SetTrigger("takeOff");
         isJumping = true;
         jumpTimeCounter = jumpTime;
         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Buddy.Jump();
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
            Buddy.StopJump();
         }
         
      }

      if (Input.GetButtonUp("Jump"))
      {
         isJumping = false;
            Buddy.StopJump();
      }
      
   }
   
   
   private void HandleDash()
   {
      if (direction == 0)
      {
         if (isDashing || PS.DashAttackOn)
         {
            isDashing = true;
            if (!facingRight) // left 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashTime);
               direction = 1;
               //JSAM.AudioManager.PlaySound(Sounds.DashLeft);
            }
            else if (facingRight) // right 
            {
               fadingGhost.createGhost = true;
               Invoke("SetCreateGhostToFalse",dashTime);
               direction = 2;
               //JSAM.AudioManager.PlaySound(Sounds.DashRight);
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
      if (isShielding && PS.ShieldCoolDown == false && PS.sensetive_Parry < 3f)
      {
         shieldBubbleSR.enabled = true;
         shieldBubbleCC2D.enabled = true;
         playerSpeed = shieldingPlayerSpeed;
         airMoveSpeed = shieldingPlayerSpeed;
         jumpForce = shieldingJumpForce;
         rb.gravityScale = 2;
      }
      else
      {
         shieldBubbleSR.enabled = false;
         shieldBubbleCC2D.enabled = false;
         playerSpeed = originalPlayerSpeed;
         airMoveSpeed = originalPlayerSpeed;
         jumpForce = originalJumpForce;
         rb.gravityScale = 4;
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
            Buddy.flip();
            Buddy.SetHitOffset();
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
      DashAttack = false;
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
      anim.SetBool("IsDashAttack", PS.DashAttackOn);
      anim.SetBool("FacingRight", facingRight);
        if (rb.velocity.y < 0 && reachedPeakJump == false)
        {
         reachedPeakJump = true;
         Invoke("SetReachedPeakToFlase", 0.05f);
        }
   }

    
   private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
           
            if (PS.DashAttacked == true)
            {
               collision.gameObject.GetComponent<Unihog1DMG>()?.killMe(dmg);
               collision.gameObject.GetComponent<HornyHogController>()?.TakeDMG(dmg);
            }
        }
       
    }

   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.gameObject.tag == "Enemy")
      {
           
         if (PS.DashAttacked == true)
         {
           print("i am here");
            other.gameObject.GetComponent<DestructableObjects>()?.HandleDestruction();
         }
      }
   }

   public void UnderImpactAnim()
    {
        print("Impact");
        anim.SetBool("IsUnderImpact", true);
        Invoke("EndImpact", 0.3f);
    }

    private void EndImpact()
    {
        anim.SetBool("IsUnderImpact", false);
    }
   
    public void PlayerDeath()
    {
        IsDead = true;
        anim.Play("Death");
        anim.SetBool("isMoving", false);
        anim.SetBool("isGrounded", false);
        anim.SetBool("isTouchingWall", false);
        anim.SetBool("isDashing", false);
        anim.SetBool("atPeakJump", false);
        anim.SetBool("IsDashAttack", false);
        anim.SetBool("FacingRight", false);
        anim.SetBool("IsUnderImpact", false);
    }

    public void HitShield()
    {
        CanShield = false; 
        Invoke("SetIsShieldingToFalse", 0.5f);
        Invoke("SetCanShieldTrue", 1.5f);
    }

    private void SetCanShieldTrue()
    {
        CanShield = true;
    }


}
