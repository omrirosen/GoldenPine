using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxNoDuplication : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    
    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x , deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPos = cameraTransform.position;

    }
}