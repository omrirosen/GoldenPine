using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using JSAM;

public class DestructableObjects : MonoBehaviour
{
   
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private GameObject destructionDustEffect;
    private SoundManager soundManager;


    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void HandleDestruction()
    {
        Instantiate(destructionEffect, transform.position, quaternion.identity);
        Instantiate(destructionDustEffect, transform.position, quaternion.identity);
        soundManager.PlayOneSound("Breaking Rock");
        Destroy(gameObject);
    }
}
