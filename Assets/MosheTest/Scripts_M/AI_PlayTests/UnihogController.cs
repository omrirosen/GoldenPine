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
    private bool bacttoIdle_Tween = false;
   

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
       
        AI_StatsControll(); // controll each state , what will it do 
        StartCoroutine(FindMyDirection()); // find ai look direction and flip GFX
        if (target_Pos != null && Mathf.Abs((target_Pos.position - transform.position).magnitude) < 5.5f)
        {
            
            seeTarget = true;
        }
        if(Chase_tween!=null)
        {
            print(Chase_tween.IsPlaying());
        }

    }

   

    public void AI_StatsControll()
    {
        switch (state)
        {
            case state_Machine.Idle:
                if (!seeTarget)
                {
                    
                    if (!Patroltween_isactive)
                    {
                        // do once
                        Patroltween_isactive = true;
                        AI_patroll_tween = transform.DOLocalPath(path, speed, PathType.Linear, PathMode.Sidescroller2D)
                            .SetEase(Ease.Flash);
                        animator.SetBool("IsMoving", Patroltween_isactive);

                    }
                    if( AI_patroll_tween.position==path[1].x)
                    {
                        print("yay");
                    }
                    if (!AI_patroll_tween.IsPlaying())
                    {

                        Patroltween_isactive = false;
                        animator.SetBool("IsMoving", Patroltween_isactive);
                    }
                }
                else
                {
                    AI_patroll_tween.Kill();
                    state = state_Machine.Attack;
                }
                break;
        
            case state_Machine.Attack:
                if (seeTarget)
                {
                    
                    
                  Chase_tween = transform.DOLocalMoveX(target_Pos.position.x, speed / 2f).SetEase(Ease.Flash);

                  if (Mathf.Abs((target_Pos.position - transform.position).magnitude) > 6f)
                  {
                  seeTarget = false;
                  }
                        // todo animations
                    
                }
                else
                {
                    StartCoroutine(ForgetTarget());
                    state = state_Machine.BackToIdle;
                }
                break;

            case state_Machine.BackToIdle:
                if (!seeTarget)
                {
                   
                    if (!bacttoIdle_Tween)
                    {
                        
                        backToIdle_tween = transform.DOLocalMoveX(path[0].x, speed / 4f).SetEase(Ease.Flash);
                        if (Mathf.Abs(path[0].x - transform.position.x) < 0.2f)
                        {
                            backToIdle_tween.Kill();
                            state = state_Machine.Idle;
                            bacttoIdle_Tween = true;
                        }
                    }
                }
                else
                {
                    backToIdle_tween.Kill();
                    bacttoIdle_Tween = false;
                    state = state_Machine.Attack;
                }

                break;
        }

        
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
           // seeTarget = true;
            target_Pos = collision.transform;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
           
        }
        else if(collision.gameObject.tag=="Block")
        {
           
            if (Chase_tween!=null)
            {
                print("kill");

                StartCoroutine(ForgetTarget());
            }
        }
    }

    

    public IEnumerator ForgetTarget()
    {
        yield return new WaitForSeconds(.5f);       
        Chase_tween.Kill();
        seeTarget = false;
       // backTo_Idle = true;

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

