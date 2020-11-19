using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using JSAM;

public class DestructableObjects : MonoBehaviour
{
   
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private GameObject destructionDustEffect;
    
    
    public void HandleDestruction()
    {
        Instantiate(destructionEffect, transform.position, quaternion.identity);
        Instantiate(destructionDustEffect, transform.position, quaternion.identity);
        JSAM.AudioManager.PlaySound(Sounds.BreakingRock);
            Destroy(gameObject);
    }
}
