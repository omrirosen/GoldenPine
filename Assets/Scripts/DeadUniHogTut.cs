using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadUniHogTut : MonoBehaviour
{
    public int numOfHitTaken = 0;

    void DestroyHog()
    {
        if (numOfHitTaken >= 3)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Update()
    {
        DestroyHog();
       // print(numOfHitTaken);
    }
}
