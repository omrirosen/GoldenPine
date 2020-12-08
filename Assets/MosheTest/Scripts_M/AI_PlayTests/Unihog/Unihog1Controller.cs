using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1Controller : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    [SerializeField]  Animator animator;
    [SerializeField] float eyes_Range ;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask eyes_Layer;
    [SerializeField] float max_Speed;
    [SerializeField] public int health;
    [SerializeField] AnimationEffects effects;
    [SerializeField] GameObject HitPartical_ins;
    [SerializeField] float wiggleTimer;
    public bool isTurning = false;
    public enum stateMachine { roming, attack,death,Flying,Wiggle };
    public stateMachine state;
    public GameObject target;
    public bool attacking = false;
    public bool isFlying = false;
    bool isWiggleOn = false;
    public float RandomSec = 0f;
    [SerializeField] float wiggleMinTime = 0.5f;
    [SerializeField] float wiggleMaxTime = 2.5f;
    public Rigidbody2D rb2d;
    float ogMoveSpeed;
    SoundManager soundManager;
    [SerializeField] GameObject noseSmokeEffect;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        state = stateMachine.roming;
        ogMoveSpeed = moveSpeed;
        soundManager = FindObjectOfType<SoundManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (health <= 0)
        {
            state = stateMachine.death;
            // Destroy(gameObject);
        }
        StateMachineControll();
        if(rb2d!=null )
        {
            
            Debug.DrawRay(transform.position + offset, (transform.TransformDirection(rb2d.velocity)).normalized*eyes_Range, Color.red);
        }
        LookForTarget();
        if (isFlying)
        {
            state = stateMachine.Flying;
        }

    }

    private void StateMachineControll()
    {
        switch (state)
        {
            case stateMachine.roming:
                
                moveSpeed = ogMoveSpeed;
                 attacking = false;
                if (!isTurning)
                {
                    if (IsFacingRight())
                    {
                        rb2d.velocity = new Vector2(moveSpeed, 0);

                        animator.SetBool("IsMoving", true);
                        effects.didPlayRollDust = false;

                    }
                    else
                    {
                        rb2d.velocity = new Vector2(-moveSpeed, 0);
                        animator.SetBool("IsMoving", true);
                        effects.didPlayRollDust = false;
                    }
                }
                else
                {
                    rb2d.velocity = Vector2.zero;
                    animator.SetBool("IsMoving", false);
                }

                break;
            case stateMachine.attack:
                wiggleTimer = 0f;
                
                attacking = true;
                isWiggleOn = false;
                animator.SetBool("isAttacking", true);
                animator.SetBool("IsWiggle", false);
                transform.localScale = new Vector2((Mathf.Sign(rb2d.velocity.x)), transform.localScale.y);
                    float dirX = target.transform.position.x - transform.position.x;
                    if (Mathf.Abs(dirX) > 0.2f)
                    {
                        rb2d.velocity = (new Vector2(dirX, 0).normalized * moveSpeed );
                        MaxRollSpeed(rb2d);
                    }
                    else
                    {
                    moveSpeed = Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                    animator.speed= Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                    //print("reach target");
                    animator.SetBool("isAttacking", false);
                    

                    }
                

                break;
            case stateMachine.death:
                
                float deathloop = animator.GetFloat("DeatLoop");
                attacking = false;
                rb2d.velocity = Vector2.zero;
                animator.SetBool("IsMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDeath", true);
                animator.SetFloat("DeatLoop", deathloop += Time.deltaTime);
                if(deathloop>=1.5f)
                {
                    
                    animator.SetBool("isDeath", false);
                }

                if (deathloop >= 3f)
                {
                    animator.SetBool("Evaporate", true);
                }
                if(deathloop>=3.5f)
                {
                    Destroy(gameObject);
                }
                break;

            case stateMachine.Flying:
                animator.Play("Unihog_NewRoll_Left");

                break;

            case stateMachine.Wiggle:
                noseSmokeEffect.SetActive(true);
                print(transform.localScale.x);
                isWiggleOn = true;
                animator.speed = 1f;
               animator.SetBool("IsMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("IsWiggle", true);
                rb2d.velocity = Vector2.zero;
                wiggleTimer += Time.deltaTime;
                if (wiggleTimer >= RandomSec)
                {
                    noseSmokeEffect.SetActive(false);
                    //attacking = true;
                    state = stateMachine.attack;
                    //soundManager.PlayOneSound("UnihogRoll");
                }




                break;
        }
    }

  

    private void MaxRollSpeed(Rigidbody2D player)
    {
        if(player.velocity.magnitude< max_Speed)
        {
            moveSpeed += 9f;
            moveSpeed += Time.deltaTime;
            animator.speed += Time.deltaTime;
        }
        else
        {
           player.velocity= player.velocity.normalized* max_Speed;
        }
    }

    public bool IsFacingRight()
    {
        return transform.localScale.x > -Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!isTurning && collision.tag=="TileMapCollider")
        {
           // print(collision.tag);
            StartCoroutine(Turn());
            transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), transform.localScale.y);
        }
    }
    

   public IEnumerator Turn()
    {
        isTurning = true;
        animator.SetBool("IsTurning", isTurning);
        yield return new WaitForSeconds(0.3f);  
        isTurning = false;
        animator.SetBool("IsTurning", isTurning);
    }

    private void LookForTarget()
    {

        if (rb2d != null || health > 0)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position + offset, transform.TransformDirection(rb2d.velocity),
                eyes_Range, eyes_Layer);
            if (hit2D.collider != null )
            {
                if (hit2D.collider.CompareTag("Player") && !attacking)
                {
                    // print("see");
                    RandomSec = Random.Range(wiggleMinTime, wiggleMaxTime);
                    target = hit2D.collider.gameObject;
                    soundManager.PlayOneSound("PigSnore1");
                    state = stateMachine.Wiggle;
                }
               
            }
            else if (hit2D.collider == null && !isFlying && !isWiggleOn)
            {
              //  print("CantSee");
                moveSpeed = Mathf.Lerp(moveSpeed, 1f, 5f*Time.deltaTime);
                animator.speed = Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                target = null;
                animator.SetBool("isAttacking", false);
                state = stateMachine.roming;
               
            }
            else if (hit2D.collider == null && isFlying)
            {
                
                state = stateMachine.Flying;
                
            }
        }
        else
        {
            state = stateMachine.death;
            
        }
       

    }


    public void killme(int dmg)
    {
        health -= dmg;
        var temp = Instantiate(HitPartical_ins, transform.position, Quaternion.identity);
       
    }

    public void JustAttacked()
    {
        state = stateMachine.roming;
    }
   
}
