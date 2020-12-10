using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleMover : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float MoveSpeed = 1;
    [SerializeField] PlayerStats playerStats;
    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player");
       playerStats = FindObjectOfType<PlayerStats>();
    }
    private void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, MoveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStats.IncreaseStamina();
            Destroy(gameObject);
        }
    }

    

}
