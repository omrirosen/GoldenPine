using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnihogController : MonoBehaviour
{
    [SerializeField] public enum state_Machine {Idle, Attack,BackToIdle }
    [SerializeField] public SpriteRenderer GFX;
    [SerializeField] public float speed = .1f;
    [SerializeField] public float patrol_distance;
    [SerializeField] public Animator animator;
    private bool timer = true;
    private state_Machine state;
    private Vector3 newPos;
    //----- Patroll ----

    public Vector3[] path;
    private bool Patroltween_isactive = false;   
    private Vector2 start_position;
    private float finalX;
    private Tween AI_patroll_tween;

    //---- patroll-------//

    // --- Chase/Attack-----//
    private Tween Chase_tween;
    private Tween backToIdle_tween;
    private Transform target_Pos;
    public bool seeTarget = false;
    public bool backTo_Idle = false;
 
    // --- Chase/Attack-----//

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
        speed = speed * 1000f*Time.deltaTime;
        
    }

    private void Update()
    {
        PickState(); // set the state 
        AI_StatsControll(); // controll each state , what will it do 
        StartCoroutine(FindMyDirection()); // find ai look direction and flip GFX



    }

    public void PickState()
    {
        if(seeTarget)
        {
            state = state_Machine.Attack;
        }
        else
        {
            state = state_Machine.Idle;
        }
        if(backTo_Idle)
        {
            state = state_Machine.BackToIdle;
        }
        else
        {
            state = state_Machine.Idle;
        }
    }

    public void AI_StatsControll()
    {
        switch (state)
        {
            case state_Machine.Idle:
                if (!Patroltween_isactive)
                {
                    // do once
                    Patroltween_isactive = true;
                    AI_patroll_tween = transform.DOLocalPath(path, speed, PathType.Linear, PathMode.Sidescroller2D)
                        .SetEase(Ease.Flash);
                    animator.SetBool("IsMoving", Patroltween_isactive);

                }
                if (!AI_patroll_tween.IsPlaying())
                {
                    
                    Patroltween_isactive = false;
                    animator.SetBool("IsMoving", Patroltween_isactive);
                }
     
                break;
        
            case state_Machine.Attack:

                Chase_tween = transform.DOLocalMoveX(target_Pos.position.x, speed/2f).SetEase(Ease.Flash);
                // todo animations
                break;

            case state_Machine.BackToIdle:
                backToIdle_tween=transform.DOLocalMoveX(path[0].x, speed / 4f).SetEase(Ease.Flash);
                //if(transform.position.x==(path[0].x-1f) || transform.position.x == (path[0].x + 1f))
                //{
                //    backTo_Idle = false; todo
                //}

                break;
        }

        
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            seeTarget = true;
            target_Pos = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target_Pos = collision.transform;
            StartCoroutine(ForgetTarget());
        }
    }

    public IEnumerator ForgetTarget()
    {
        yield return new WaitForSeconds(.5f);       
        Chase_tween.Kill();
        seeTarget = false;
        backTo_Idle = true;

    }

    private IEnumerator FindMyDirection()
    {
        if (timer)
        {
            timer = false;
            newPos = transform.position;
            
        }

        yield return new WaitForSeconds(.1f);
        timer = true;

        if (newPos.x < transform.position.x)
        {
        GFX.GetComponent<SpriteRenderer>().flipX = true;

        }

        else if (newPos.x > transform.position.x)
        {
          GFX.GetComponent<SpriteRenderer>().flipX = false;

        }

    } 
}

