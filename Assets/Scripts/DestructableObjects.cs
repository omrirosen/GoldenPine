using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DestructableObjects : MonoBehaviour
{
   
    [SerializeField] private GameObject destructionEffect;
    
    
    public void HandleDestruction()
    {
        Instantiate(destructionEffect, transform.position, quaternion.identity);
            Destroy(gameObject);
    }
}
