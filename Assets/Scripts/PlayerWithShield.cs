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
   private float yVelocity;

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
  [SerializeField] private bool isWallSliding;
   
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
    [SerializeField] private GameObject WhitePraticale;
    [SerializeField] private GameObject whiteParticleFX;
    public bool isShielding;
    [SerializeField] private PlayerStats PS;
    private bool isCharged = false;

    private SpriteRenderer playerSpriteRenderer;

    // Component Caches
    [SerializeField] SoundManager soundManager;
   private Rigidbody2D rb;
   private Animator anim;
   public BuddyController Buddy;
   private CollisionCheck collisionCheck;
   [SerializeField] private CinemachineImpulseSource pulseSource;
   public bool DashAttack = false;
    [SerializeField] GameObject ChargAnim;
    public bool IsDead = false;
    bool CanShield = true;
    GameObject Player;
    bool DashCharging = false;
    GameManager GM;
    public bool canPierceDash = false;
    bool isUnderImpact = false; 
    //[SerializeField] GameObject DustRun;
    [SerializeField] private GameObject whiteUiParticleEffect;
    bool wallSlideSoundIsPlaying = false;
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
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        canPierceDash = false;
        soundManager = FindObjectOfType<SoundManager>();
    }

   private void Start()
   {
      dashTime = startDashTime;
      wallJumpAngle.Normalize();
      shieldBubbleSR.enabled = false;
      whiteUiParticleEffect.SetActive(false);
      soundManager.PlayOneSound("Respawn");
      GM.populateRefrences();
        
   }

   private void Update()
   {
      if(IsDead == false && isUnderImpact == false) 
      {
          Inputs();
          PlayerJump();
          PlayerMovement();
          HandleShield();
          WallSlide();
          AnimationSetup();
          SetChargeAnime();
          LookTowardWall();
      }
      else if(IsDead == true)
      {
            Invoke("OnDeath", 2f);
            
      }

   }

   private void FixedUpdate()
   {
        if (IsDead == false && isUnderImpact == false)
        {
            HandleDash();
        }
   }
    
   private void OnDeath()
    {
        GM.ResetScene();
    }

   private void Inputs()
   
   {
      //Horizontal Inputs
      xMoveInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw meaning snappy movement, remove raw for fluidity
        yVelocity = rb.velocity.y;                                            //Dash Inputs
      if (Input.GetKeyDown(KeyCode.LeftShift) && DashCooldown == false)
      {
            soundManager.PlayDashArray();
            DashCooldown = true;
            isDashing = true;
            Invoke("ResetDashCoolDown", 0.5f);
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            Buddy.Dash();
            if (isShielding)
            {
                isShielding = false;
                HitShield();
            }
      }
      if(Input.GetKey(KeyCode.Z) && PS.DashStock >= 1 && canPierceDash)
      {
          anim.Play("DashAttack");
            canPierceDash = false;
            Invoke("ChargedTofalse", 0.5f);
            PS.DashAttackOn = true;
            dashSpeed = 20f;
            Invoke("BackToOGDashSpeed", 0.2f);
            anim.SetBool("IsWhite", false);
            WhitePraticale.SetActive(false);
            whiteUiParticleEffect.SetActive(false);
            soundManager.PlayOneSound("Pircieng Dash");
           // whiteParticleFX.SetActive(false);
        }
      //Shield Inputs
      
      if (Input.GetKeyDown(KeyCode.X))
      {
         isShielding = true;
         Invoke("DidShieldTrue", 0.05f);
      }
      
      if(Input.GetKeyUp(KeyCode.X))
      {
            isShielding = false;
            anim.SetBool("DidShield", false);
            // Invoke("SetIsShieldingToFalse",0.3f);
      }
   }
    private void DidShieldTrue()
    {
        anim.SetBool("DidShield", true);
    }
    private void ChargedTofalse()
    {
        isCharged = false;
    }

    private void BackToOGDashSpeed()
    {
        dashSpeed = 10f;
        PS.DashAttackOn = false;
        isDashing = false;
       
    }
    public void GeneratPulse()
    {
        pulseSource.GenerateImpulse();
       
    }

   
    private void ResetDashCoolDown()
    {
        DashCooldown = false;
    }
    
  /*
    private void SetDashChagingFalse()
    {
        if (PS.DashStock >= 1 && DashCharging == true)
        {
            ChargAnim.SetActive(true);
            Invoke("SetWhite", 0.5f);
            DashCharging = false;
            
        }
    }
    */
    private void SetChargeAnime()
    {
        if (PS.DashStock >= 1 && isCharged == false)
        {
            whiteUiParticleEffect.SetActive(true);
            ChargAnim.SetActive(true);
            Invoke("SetWhite", 0.5f);
            isCharged = true;
            WhitePraticale.SetActive(true);
           // whiteParticleFX.SetActive(true);
        }
    }
   public void SetWhite()
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
            //if (collisionCheck.onGround) DustRun.SetActive(true);
            //if (collisionCheck.onGround == false || isUnderImpact) DustRun.SetActive(false);
      }
      else
      {
         isMoving = false;
            Buddy.StopRun();
           // DustRun.SetActive(false);
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
      if (xMoveInput < 0 && facingRight && isDashing ==false )
      {
         FlipSprite();
      }
      else if (xMoveInput > 0 && !facingRight && isDashing == false)
      {
         FlipSprite();
      }
      
      // if on ground
      if (collisionCheck.onGround)
      {
         reachedPeakJump = false;
      }

   }

   private void PlayerJump()
   {
      
      if (Input.GetButtonDown("Jump") && collisionCheck.onGround)
      {
          soundManager.PlayOneSound("Jump");
         anim.SetTrigger("takeOff");
         isJumping = true;
         jumpTimeCounter = jumpTime;
         rb.velocity = new Vector2(rb.velocity.x, jumpForce);
          if(!isShielding) Buddy.Jump();
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
                if (collisionCheck.onWall == false)
                {
                    isDashing = true;
                    if (!facingRight) // left 
                    {
                        fadingGhost.createGhost = true;
                        Invoke("SetCreateGhostToFalse", dashTime);
                        direction = 1;
                    }
                    else if (facingRight) // right 
                    {
                        fadingGhost.createGhost = true;
                        Invoke("SetCreateGhostToFalse", dashTime);
                        direction = 2;
                    }
                }
                else if(collisionCheck.onWall == true)
                {
                    if (collisionCheck.onRightWall)
                    {
                        fadingGhost.createGhost = true;
                        Invoke("SetCreateGhostToFalse", dashTime);
                        direction = 1;
                        FlipSprite();
                    }
                    if (collisionCheck.onLeftWall)
                    {
                        fadingGhost.createGhost = true;
                        Invoke("SetCreateGhostToFalse", dashTime);
                        direction = 2;
                        FlipSprite();
                    }
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
                Invoke("SetIsDashingToFalse", 0.2f);
            }
            else
            {
                fadingGhost.createGhost = true;
                Invoke("SetCreateGhostToFalse", dashTime);
                dashTime -= Time.deltaTime;
                if (direction == 1)
                {
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.left * dashSpeed;
                    Invoke("SetIsDashingToFalse", 0.2f);

                }
                else if (direction == 2)
                {
                    rb.gravityScale = 0;
                     rb.velocity = Vector2.right * dashSpeed;
                     Invoke("SetIsDashingToFalse",0.2f);
                }
            }

      }
      
   }

   void HandleShield()
   {
      if (isShielding && !isDashing)
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
         //shieldBubbleSR.enabled = false;
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
            if (!wallSlideSoundIsPlaying)
            {
                soundManager.PlayOneSound("Wall Slide");
                wallSlideSoundIsPlaying = true;
            }
          
         
      }
      else
      {
        soundManager.StopOneSound("Wall Slide");
         isWallSliding = false;
         anim.SetBool("StartSlide", false);
         wallSlideSoundIsPlaying = false;
      }

      if (isWallSliding)
      {
         rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
      }

      if (collisionCheck.onRightWall && xMoveInput < 0 || collisionCheck.onLeftWall && xMoveInput > 0)
      {
         isWallSliding = false;
         anim.SetBool("StartSlide", false);
      }
   }
    private void LookTowardWall()
    {
        if (isWallSliding == true)
        {
            if (collisionCheck.onLeftWall)
            {
                if (facingRight)
                {
                    FlipSprite();
                }
            }

            if (collisionCheck.onRightWall)
            {
                if (!facingRight)
                {
                    FlipSprite();
                }
            }
            
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
      
         wallJumpDirection *= -1;
         //playerSpriteRenderer.flipX = facingRight;
         facingRight = !facingRight;
         
         transform.Rotate(0, 180, 0);
         
         Buddy.flip();
            //Buddy.SetHitOffset();
            //Buddy.FlipAttackPoint();
     
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
      anim.SetFloat("YVelocity", yVelocity);
      anim.SetBool("isShielding", isShielding);
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
                soundManager.PlayOneSound("Hit Enemy");
               collision.gameObject.GetComponent<UnihogGettingHit>()?.killMe(dmg);
               collision.gameObject.GetComponent<HornyHogTakeDmg>()?.TakeDMG(dmg);
               
            }
        }
       
    }

   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.gameObject.tag == "Enemy")
      {
           
         if (PS.DashAttacked == true)
         {
          // print("i am here");
            other.gameObject.GetComponent<DestructableObjects>()?.HandleDestruction();
         }
      }
   }

   public void UnderImpactAnim()
    {
       // print("Impact");
        anim.SetBool("IsUnderImpact", true);
        isUnderImpact = true;
        Invoke("EndImpact", 0.3f);
        
    }

    private void EndImpact()
    {
        anim.SetBool("IsUnderImpact", false);
        isUnderImpact = false;
    }
   
    public void PlayerDeath()
    {
        Buddy.playerIsDead = true;
        print("dead");
        IsDead = true;
       soundManager.PlayOneSound("Death");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        anim.SetBool("IsDead", true);
        anim.SetBool("isMoving", false);
        anim.SetBool("isGrounded", false);
        anim.SetBool("isTouchingWall", false);
        anim.SetBool("isDashing", false);
        anim.SetBool("atPeakJump", false);
        anim.SetBool("IsDashAttack", false);
        anim.SetBool("FacingRight", false);
        anim.SetBool("IsUnderImpact", false);
        Buddy.BuddyDie();
        
       
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
