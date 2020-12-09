using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        Player.SetActive(false);
    }
    private void Start()
    {
        if (gameManager.passedCertainCheckpoint)
        {
            transform.position = gameManager.lastCheckpointReached;   
        }
        else
        {
            transform.position = transform.position;
        }
        Player.transform.position = transform.position;
    }

    public void RespawnPlayer()
    {
        Player.SetActive(true);
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);
    }
}
