using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogDMG : MonoBehaviour
{
    [SerializeField] int DMG;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D dmg_Collider;
    [SerializeField] HornyHogController hornyHogController;
    public bool IsFachingRight;
    public bool isdealDMG = false;
    public bool isActive = false;
    public bool isBlocked = false;
    private bool isAttackingNow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dmg_Collider = GetComponent<BoxCollider2D>();
        hornyHogController = GetComponentInParent<HornyHogController>();
    }

   

    // Update is called once per frame
    void Update()
    { 
        if (isActive)
        {
            dmg_Collider.enabled = true;
            hornyHogController.isAttacking = true;
        }
        else
        {
            dmg_Collider.enabled = false; 
            hornyHogController.isAttacking = false;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hornyHogController.isAttacking)
            {
                if (!isdealDMG)
                {
                    if (IsFachingRight)
                    {
                        //print("entered facing right state");
                        isdealDMG = true;
                        if (collision.GetComponent<PlayerStats>() != null)
                        {
                            collision.GetComponent<PlayerStats>().TakeDmg(DMG, Vector3.left);
                            if (collision.GetComponent<PlayerStats>().shieldOn ||
                                collision.GetComponent<PlayerStats>().ParryWindow)
                            {
                                isBlocked = true;
                                animator.SetBool("IsBlocked", isBlocked);
                                Invoke("resetBlock", 0.2f);
                            }

                        }
                        else
                        {
                            collision.GetComponent<ShieldBubble>().ConnectToTakeDMG(DMG, Vector3.left);
                            if (collision.GetComponent<ShieldBubble>().ConnectToShidStatus())
                            {
                                isBlocked = true;
                                animator.SetBool("IsBlocked", isBlocked);
                                Invoke("resetBlock", 0.2f);
                            }

                        }
                    }
                    else
                    {
                        // print("entered facing left");
                        isdealDMG = true;
                        if (collision.GetComponent<PlayerStats>() != null)
                        {
                            collision.GetComponent<PlayerStats>()?.TakeDmg(DMG, Vector3.right);
                            if (collision.GetComponent<PlayerStats>().shieldOn ||
                                collision.GetComponent<PlayerStats>().ParryWindow)
                            {
                                isBlocked = true;
                                animator.SetBool("IsBlocked", isBlocked);
                                Invoke("resetBlock", 0.2f);
                            }
                        }
                        else
                        {
                            collision.GetComponent<ShieldBubble>().ConnectToTakeDMG(DMG, Vector3.right);
                            if (collision.GetComponent<ShieldBubble>().ConnectToShidStatus())
                            {
                                isBlocked = true;
                                animator.SetBool("IsBlocked", isBlocked);
                                Invoke("resetBlock", 0.2f);
                            }

                        }
                    }
                }
            }
        }
    }

    public void resetBlock()
    {
        isBlocked = false;
        animator.SetBool("IsBlocked", isBlocked);
    }

    public void ResetChoice()
    {
        hornyHogController.choseAttack = false;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsSwingAttack", false);
    }
  


}
