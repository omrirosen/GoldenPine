using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCeleste : MonoBehaviour
{
    private CollisionCheck collisionCheck;

    [HideInInspector] public Rigidbody2D rb;
    private PlayerAnimation anim;

    [Space] [Header("Stats")] public float playerSpeed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    [Space] [Header("Booleans")] public bool canMove;
    public bool wallTouch;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space] private bool groundTouch;
    private bool hasDashed;

    private float xMoveInput;
    private float jumpTime = 0.35f;
    private float jumpTimeCounter;
    

    public int side = 1; //facing right

    private void Start()
    {
        collisionCheck = GetComponent<CollisionCheck>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerJump();
        WallSlide();
    }

    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(x, y);
        Run(direction);
        
        // for flipping
        if (xMoveInput < 0 && side == 1 )
        {
            FlipSprite();
        }
        else if (xMoveInput > 0 && side == -1)
        {
            FlipSprite();
        }
        
    }

    private void Run(Vector2 direction)
    {
        rb.velocity = (new Vector2(direction.x * playerSpeed, rb.velocity.y));
    }

    private void PlayerJump()
    {
      
        if (Input.GetButtonDown("Jump") && collisionCheck.onGround)
        {
            anim.SetTrigger("takeOff");
            groundTouch = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Jump") && !groundTouch)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                groundTouch = true;
            }
         
        }

        if (Input.GetButtonUp("Jump"))
        {
            groundTouch = true;
        }
        
    }

    private void WallSlide()
    {
        if (collisionCheck.onWall && !collisionCheck.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }
    }

    private void WallJump()
    {
        if ((side == 1 && collisionCheck.onRightWall) || side == -1 && !collisionCheck.onRightWall)
        {
            side *= -1;
            FlipSprite();
        }
    }
    
    void FlipSprite()
    {
         if (!collisionCheck.onWall)
         {
        side *= -1;
       // facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
         }
    }
    
}
