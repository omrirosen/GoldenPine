using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 4;
    private float DashStock = 0;
    [SerializeField] private GameObject PlayerSelf;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator StaminaAnimator;
    [SerializeField] private SpriteRenderer sRenderer;
    bool shieldOn = false;
    public bool ParryWindow = false;
    private Color OGcolor;
    public Color DashColor;
    public bool DashAttacked = false;
    private PlayerWithShield playerWithShield;
    [SerializeField] private GameObject Buddy;


    private bool isImpect_ON = false;
    private Rigidbody2D rb2d;
    [SerializeField] float impact_Force;
    [SerializeField] float impact_JumpForce;
    private Tween Impact;

    private void Awake()
    {
        OGcolor = sRenderer.color;
        playerWithShield = this.GetComponent<PlayerWithShield>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
       
        HealPlayer();
        ShieldUp();
        Dashed();
        if (Impact != null)
        {
            if (!Impact.IsPlaying())
            {
                isImpect_ON = false;
            }
        }
    }

    public void TakeDmg(int Dmg,Vector3 dir)
    {
        if (shieldOn == false && ParryWindow == false && playerWithShield.isDashing == false)
        {
            playerHealth -= Dmg;
            animator.SetInteger("PlayerHealthUI", playerHealth);
            if (!isImpect_ON)
            {
                isImpect_ON = true;
                Impact = rb2d.DOJump(transform.position - dir * impact_Force, impact_JumpForce, 0, 0.5f);
                Impact.SetEase(Ease.Flash);
            }
        }

       if ( ParryWindow == true) 
       {
            
            Parry();
       }

       if (playerHealth <= 0)
       {
            Die();
       }
    }

    private void Die()
    {
       
        PlayerSelf.SetActive(false);
    }
    
    public void HealPlayer()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (playerHealth == 1)
            {
                animator.Play("IncreaseHealth");
                playerHealth = 4;
            }

            if(playerHealth == 2)
            {
                animator.Play("IncreaseHealth_2");
                playerHealth = 4;
            }

            if(playerHealth == 3)
            {
                animator.Play("IncreaseHealth_3");
                playerHealth = 4;
            }
            animator.SetInteger("PlayerHealthUI", playerHealth);
        }
    }

    private void ShieldUp()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            shieldOn = true;
            Buddy.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            shieldOn = false;
            ParryWindow = true;
            Buddy.SetActive(true);
            Invoke("ParryEnd", 0.5f);

        }
    }
    
    private void ParryEnd()
    {
        ParryWindow = false;
    }
    
    private void Parry()
    {
       
        if (DashStock < 1)
        {
            DashStock = DashStock +1;
            StaminaAnimator.SetFloat("StaminaUi", DashStock);
        }
       
        
    }

    private void RestTime()
    {
        
    }

    private void Dashed()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z) && DashStock >= 1)
        {
            DashStock = DashStock - 1;
            StaminaAnimator.SetFloat("StaminaUi", DashStock);
            StaminaAnimator.Play("StaminaDown");
            sRenderer.color = DashColor ;
            Invoke("BackToOGColor", 0.5f);
            DashAttacked = true;
            playerWithShield.isDashing = true;
        }
    }

    private void BackToOGColor()
    {
        sRenderer.color = OGcolor;
        DashAttacked = false;
        playerWithShield.isDashing = false;
    }

}
