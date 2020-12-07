using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        Player.SetActive(false);
        Player.transform.position = this.transform.position;
    }

    public void RespawnPlayer()
    {
        Player.SetActive(true);
    }

    public void DestroySelf()
    {
        this.gameObject.SetActive(false);
    }
}
