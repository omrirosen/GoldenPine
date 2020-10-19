using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyController : MonoBehaviour
{
    private GameObject Self;
    [SerializeField] private Animator Anim;
    private Collider2D AttackColl;
    bool IsJumping = false;
    bool JustAttacked = false;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Self = this.gameObject;
        AttackColl = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        attack();
        print(JustAttacked);
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

    private void attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (JustAttacked == true)
            {
                print("hit2");
                Anim.Play("buddy_hit2");
                JustAttacked = false;
            }

            if (JustAttacked == false)
            {
                print("hit1");
                Anim.Play("Buddy_hit1");
                JustAttacked = true;
                //Invoke("ComboTimeOut", 0.3f);
            }

        }
    }

    private void ComboTimeOut()
    {
        JustAttacked = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
