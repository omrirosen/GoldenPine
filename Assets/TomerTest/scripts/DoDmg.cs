using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDmg : MonoBehaviour
{
    private PlayerStats PS;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player With Shield");
       PS = player.GetComponent<PlayerStats>();
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PS.TakeDmg(1);
        }
    }

    
}
