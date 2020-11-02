using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private float cloudSpeed;
    private float cloudEndPosX;

    private void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * cloudSpeed));
        if (transform.position.x > cloudEndPosX)
        {
            Destroy(gameObject);
        }
    
    }

    public void StartFloating(float speed, float endPosX)
    {
        cloudSpeed = speed;
        cloudEndPosX = endPosX;

    }
}
