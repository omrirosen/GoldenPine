using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnihogKickDetection : MonoBehaviour
{
    Unihog1Controller unihog1Controller;

    private void Awake()
    {
        unihog1Controller = GetComponentInParent<Unihog1Controller>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            unihog1Controller.NeedForKick();
            print("Player Enterd Collisoin");
        }
    }
 

}
