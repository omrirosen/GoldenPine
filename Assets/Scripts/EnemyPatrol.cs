using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed;
    private int currentPointIndex;
    private float waitTime;
    public float startWaitTime;

    private void Start()
    {
        transform.position = patrolPoints[0].position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position,
            speed * Time.deltaTime);
        if (transform.position == patrolPoints[currentPointIndex].position)
        {
            if (currentPointIndex + 1 < patrolPoints.Length)
            {
                currentPointIndex++; 
            }
            else
            {
                currentPointIndex = 0;
            }
            
        }
    }
}
