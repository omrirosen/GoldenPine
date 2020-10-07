using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnihogController : MonoBehaviour
{
    [SerializeField] public enum state_Machine {Idle, Attack }
    [SerializeField] public SpriteRenderer GFX;
    [SerializeField] public float speed = .1f;
    [SerializeField] public float patrol_distance;

    public Vector3[] path;
    
    private state_Machine state;

    public bool isactive = false;
    public Animator animator;
    private Vector2 start_position;
    private float finalX;
    private Tween AI_patroll_tween;

    private void Start()
    {
        start_position = transform.localPosition;
        // init patrol points based on distance
        path[0] = start_position;
        path[1] = new Vector2(start_position.x + patrol_distance, start_position.y);
        path[2] = new Vector2(start_position.x - patrol_distance, start_position.y);
        path[path.Length-1] = start_position;
        state = state_Machine.Idle;
        // fix speed
        speed = speed * 1000f;
        
    }

    private void Update()
    {
        AI_StatsControll();
        FlipSprite();
        animator.SetBool("IsMoving", isactive);

    }

    public void AI_StatsControll()
    {
        switch (state)
        {
            case state_Machine.Idle:
                if (!isactive)
                {
                    // do once
                    isactive = true;
                    AI_patroll_tween = transform.DOLocalPath(path,Time.deltaTime* speed, PathType.CatmullRom,PathMode.Sidescroller2D);
                    
                }
                if (!AI_patroll_tween.IsPlaying())
                {
                    // reset tween for loop
                    AI_patroll_tween.PlayBackwards();
                    isactive = false;
                }
               
                
                
                break;
        
            case state_Machine.Attack:
                break;
            default:
                break;
        }

        
        
    }

    private void FlipSprite()
    {
        if (transform.localPosition.x > start_position.x)
        {
            GFX.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GFX.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    
}

