using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogDMG : MonoBehaviour
{
    [SerializeField] int DMG;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D dmg_Collider;
    public bool IsFachingRight;
    public bool isdealDMG = false;
    public bool isActive = false;
    public bool isBlocked = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dmg_Collider = GetComponent<BoxCollider2D>();
    }

   

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            
            dmg_Collider.enabled = true;
        }
        else
        {
            dmg_Collider.enabled = false;
           
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (!isdealDMG)
            {
                if (IsFachingRight)
                {
                    isdealDMG = true;
                    collision.GetComponent<PlayerStats>()?.TakeDmg(DMG, Vector3.left);
                    if(collision.GetComponent<PlayerStats>().shieldOn || collision.GetComponent<PlayerStats>().ParryWindow)
                    {
                        isBlocked = true;
                        animator.SetBool("IsBlocked", isBlocked);
                    }
                }
                else
                {
                    isdealDMG = true;
                    collision.GetComponent<PlayerStats>()?.TakeDmg(DMG, Vector3.right);
                    if (collision.GetComponent<PlayerStats>().shieldOn || collision.GetComponent<PlayerStats>().ParryWindow)
                    {
                        isBlocked = true;
                        animator.SetBool("IsBlocked", isBlocked);
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



}
