using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDmg : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats PS = collision.GetComponent<PlayerStats>();
        if(PS != null)
        {
            PS.TakeDmg(4, Vector3.zero);
        }
    }
}
