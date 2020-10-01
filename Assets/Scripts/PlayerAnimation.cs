using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerCeleste playerCeleste;
    private CollisionCheck collisionCheck;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        collisionCheck = GetComponentInParent<CollisionCheck>();
        playerCeleste = GetComponentInParent<PlayerCeleste>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("onGround", collisionCheck.onGround);
        anim.SetBool("onWall", collisionCheck.onWall);
        anim.SetBool("onRightWall", collisionCheck.onRightWall);
        /*
        anim.SetBool("wallGrab", playerCeleste.wallGrab);
        anim.SetBool("wallSlide", playerCeleste.wallSlide);
        anim.SetBool("canMove", playerCeleste.canMove);
        anim.SetBool("isDashing", playerCeleste.isDashing);
        */

    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    /*
    public void Flip(int side)
    {

        if (playerCeleste.wallGrab || playerCeleste.wallSlide)
        {
            if (side == -1 && spriteRenderer.flipX)
                return;

            if (side == 1 && !spriteRenderer.flipX)
            {
                return;
            }
        }

        bool state = (side == 1) ? false : true;
        spriteRenderer.flipX = state;
    }
    */
}
