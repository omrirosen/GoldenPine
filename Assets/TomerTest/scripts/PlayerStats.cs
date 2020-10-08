using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 4;
    [SerializeField] private GameObject PlayerSelf;
    
    public void TakeDmg(int Dmg)
    {
        playerHealth -= Dmg;
        Debug.Log(playerHealth);
        Debug.Log(Dmg);

        if (playerHealth <= 0)
        {
            Die();

        }

        
    }
    private void Die()
    {
        PlayerSelf.SetActive(false);
    }
}
