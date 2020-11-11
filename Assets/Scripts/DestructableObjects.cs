using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DestructableObjects : MonoBehaviour
{
   
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private GameObject destructionDustEffect;
    
    
    public void HandleDestruction()
    {
        Instantiate(destructionEffect, transform.position, quaternion.identity);
        Instantiate(destructionDustEffect, transform.position, quaternion.identity);
            Destroy(gameObject);
    }
}
