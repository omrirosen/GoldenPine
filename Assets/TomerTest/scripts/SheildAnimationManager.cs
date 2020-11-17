using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildAnimationManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer shieldSR;

    private void Awake()
    {
        shieldSR = GetComponent<SpriteRenderer>();
    }
    public void VanishShield()
    {
        shieldSR.enabled = false;
    }
}
