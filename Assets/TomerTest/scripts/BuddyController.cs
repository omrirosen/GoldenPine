using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;

public class BuddyController : MonoBehaviour
{
    private GameObject Self;
    [SerializeField] private Animator Anim;
    private SpriteRenderer SRenderer;
    private Collider2D AttackColl;
    bool IsJumping = false;
    bool JustAttacked = false;
    int NumOfClicks = 0;
    float LastClickedTime = 0f;
    float MaxComboDelay = 0.4f;
    [SerializeField] private Transform AttackTransform;
    [SerializeField]private Transform OGTransform;
    [SerializeField]float Speed = 1f;
    float MoveSpeed;
    [SerializeField] PlayerStats playerstats;
    public Vector2 Offset;
    public LayerMask AIlayer;
    public Vector2 OffsetLeft;
    public Vector2 OffsetRight;
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Self = this.gameObject;
        AttackColl = GetComponent<BoxCollider2D>();
        SRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
       MoveSpeed = Speed * Time.deltaTime;
    }

    public void flip()
    {
        Anim.Play("Buddy_flip");
    }

    public void Dash()
    {
        Anim.Play("Buddy_dash");
    }

    public void Run()
    {
       
        Anim.SetBool("IsMoving", true);
        
    }

    public void StopRun()
    {
        Anim.SetBool("IsMoving", false);
    }
   
    public void Jump()
    {
        Anim.SetBool("IsJumping", true);
        IsJumping = true;
    }

    public void StopJump()
    {
        Anim.SetBool("IsJumping", false);
        IsJumping = false;
    }

    private void Attack()
    {
        if(Time.time - LastClickedTime > MaxComboDelay)
        {
            NumOfClicks = 0;
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            LastClickedTime = Time.time;
            NumOfClicks++;
            
            SRenderer.sortingOrder = 10;
            Invoke("ResetSortingOrder", 0.7f);
            if(NumOfClicks == 1)
            {
                Anim.SetBool("Attack1", true);
                MoveToAttackPoint();
                Invoke("BackToOGPos", 0.5f);
                Invoke("SetAttack1False", 0.1f);
                Collider2D Enemy = Physics2D.OverlapCircle((Vector2)transform.position + Offset, 0.25f, AIlayer);
                //Physics2D.OverlapCircle((Vector2)transform.position + Offset, 0.25f, AIlayer);
                if (Enemy != null)
                {
                    playerstats.IncreaseStamina();
                }
            }
            

            if (NumOfClicks == 2)
            {
                Anim.SetBool("Attack2", true);
                Anim.SetBool("Attack1", false);
                MoveToAttackPoint();
                //Physics2D.OverlapCircle((Vector2)transform.position + Offset, 0.25f, AIlayer);
                Collider2D Enemy = Physics2D.OverlapCircle((Vector2)transform.position + Offset, 0.25f, AIlayer);
                Enemy.GetComponent<Unihog1Controller>();
                if(Enemy != null)
                {
                    playerstats.IncreaseStamina();
                }
            }
        }

        if(NumOfClicks == 0)
        {
            Anim.SetBool("Attack1", false);
            Anim.SetBool("Attack2", false);
            BackToOGPos();
        }
        NumOfClicks = Mathf.Clamp(NumOfClicks, 0, 1);
    }

    private void SetAttack1False()
    {
        Anim.SetBool("Attack1", false);
    }

    private void MoveToAttackPoint()
    {
        Self.transform.position = AttackTransform.position;
    }

    private void BackToOGPos()
    {
        
        Self.transform.position = Vector3.MoveTowards(Self.transform.position, OGTransform.position , MoveSpeed );
    }

    private void ResetSortingOrder()
    {
        SRenderer.sortingOrder = 1;
    }

    private void ComboTimeOut()
    {
        JustAttacked = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + Offset, 0.25f);
    }

    public void SetHitOffset()
    {
        if(Offset == OffsetRight)
        {
            Offset = OffsetLeft;
        }

        if (Offset == OffsetLeft)
        {
            Offset = OffsetRight;
        }
    }
}



