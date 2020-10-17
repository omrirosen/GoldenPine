using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyController : MonoBehaviour
{
    private GameObject Self;
    [SerializeField] private Animator Anim;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Self = this.gameObject;
        
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
        Anim.Play("Buddy_run");
        Anim.SetBool("IsMoving", true);
    }

    public void StopRun()
    {
        Anim.SetBool("IsMoving", false);
    }
   
    
}
