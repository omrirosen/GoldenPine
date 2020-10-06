using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnihogController : MonoBehaviour
{
    [SerializeField] public enum state_Machine {Idle, SeeTarget, Attack }
    [SerializeField] public SpriteRenderer GFX;
    private state_Machine state;
    public float speed = 5f;

    private void Start()
    {
        state = state_Machine.Idle;
    }

    private void Update()
    {
        AI_StatsControll();
    }

    public void AI_StatsControll()
    {
        switch (state)
        {
            case state_Machine.Idle:
            //    transform.DOMoveX(transform.position.x - 10f, 2f*speed*Time.deltaTime);
                break;
            case state_Machine.SeeTarget:
                break;
            case state_Machine.Attack:
                break;
            default:
                break;
        }
    }
}

