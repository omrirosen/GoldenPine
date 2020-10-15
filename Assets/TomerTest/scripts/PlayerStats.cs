using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    private void Awake()
    {
        OGcolor = sRenderer.color;
    }
    private void Update()
    {
        HealPlayer();
        ShieldUp();
        Dashed();
    }

    public void TakeDmg(int Dmg)
    {
        if (shieldOn == false && ParryWindow == false)
        {
            playerHealth -= Dmg;
            animator.SetInteger("PlayerHealthUI", playerHealth);
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
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            shieldOn = false;
            ParryWindow = true;
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
        }
    }

    private void BackToOGColor()
    {
        sRenderer.color = OGcolor;
    }

}
