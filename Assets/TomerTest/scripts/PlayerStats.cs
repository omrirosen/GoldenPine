using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 4;
    [SerializeField] private GameObject PlayerSelf;
    [SerializeField] private Animator animator;
    
    public void TakeDmg(int Dmg)
    {
        playerHealth -= Dmg;
        Debug.Log(playerHealth);

        animator.SetInteger("PlayerHealthUI", playerHealth);

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

    private void Update()
    {
        HealPlayer();
    }


}
