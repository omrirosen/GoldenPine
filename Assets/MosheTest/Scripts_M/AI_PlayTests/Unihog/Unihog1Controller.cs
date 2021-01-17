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
    [SerializeField] float chaseTimer;
    public bool isTurning = false;
    public enum stateMachine { roming, attack, death, Flying, Wiggle, Chase, Kick};
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
    EnemySoundManager enemySoundManager;
    [SerializeField] GameObject noseSmokeEffect;
    [SerializeField] GameObject player;
    [SerializeField] bool hasSpotedPlayer = false;
    public bool kicking = false;
    bool test = false;
    float targetLastSeen_posX;
    [SerializeField] GameObject Twinkle;
    [SerializeField] ParticleSystem leafParticle;
    [SerializeField] EnemySpawner enemySpawner;
    private bool scoreCalculated = false;
    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    void Start()
    {
        if (enemySpawner != null)
        {
            enemySpawner.numbOfEnemies++;
        }
        rb2d = GetComponent<Rigidbody2D>();
        state = stateMachine.roming;
        ogMoveSpeed = moveSpeed;
        enemySoundManager = GetComponent<EnemySoundManager>();
        player = GameObject.FindGameObjectWithTag("Player");
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
            leafParticle.Stop();
            state = stateMachine.Flying;
        }
        FaceTarget();
        
    }

    private void StateMachineControll()
    {
        switch (state)
        {
            case stateMachine.roming:
                leafParticle.Stop();
                chaseTimer = 0f;
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
                    if (Mathf.Abs(dirX) > 0.002f)
                    {
                        rb2d.velocity = (new Vector2(dirX, 0).normalized * moveSpeed );
                        MaxRollSpeed(rb2d);
                    }
                    else
                    {
                        moveSpeed = Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                        animator.speed= Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                   
                        animator.SetBool("isAttacking", false);

                    }
                
                

                break;
            case stateMachine.death:
                leafParticle.Stop();
                target = null;
                float deathloop = animator.GetFloat("DeatLoop");
                attacking = false;
                noseSmokeEffect.SetActive(false);
                rb2d.velocity = Vector2.zero;
                animator.SetBool("isKicking", false);
                animator.SetBool("IsMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDeath", true);
                animator.SetFloat("DeatLoop", deathloop += Time.deltaTime);
                if (!scoreCalculated)
                {
                    enemySpawner.IncreasScore();
                    scoreCalculated = true;
                }
                if (deathloop>=1.5f)
                {
                    
                    animator.SetBool("isDeath", false);
                }

                if (deathloop >= 3f)
                {
                    animator.SetBool("Evaporate", true);
                }
                if(deathloop>=3.5f)
                {
                    if (enemySpawner != null)
                    {
                        enemySpawner.numbOfEnemies--;
                        enemySpawner.enemiesDefeated++;
                    }
                    Destroy(gameObject);
                }
                break;

            case stateMachine.Flying:
                animator.Play("Unihog_NewRoll_Left");
                attacking = false;
                break;

            case stateMachine.Wiggle:
                leafParticle.Stop();
                chaseTimer = 0f;
                noseSmokeEffect.SetActive(true);

                isWiggleOn = true;
                animator.speed = 1f;
                animator.SetBool("isKicking", false);
                animator.SetBool("IsMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("IsWiggle", true);
                rb2d.velocity = Vector2.zero;
                wiggleTimer += Time.deltaTime;
                if (wiggleTimer >= RandomSec - 0.3f)
                {
                    Twinkle.SetActive(true);
                    if (wiggleTimer >= RandomSec)
                    {
                        noseSmokeEffect.SetActive(false);
                        Twinkle.SetActive(false);
                        state = stateMachine.attack;
                        leafParticle.Play();
                    }
                }
                break;

            case stateMachine.Kick:
                
                StartCoroutine("KickPlayer");
                animator.SetBool("IsMoving", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("IsWiggle", false);
                
                target = null;
                if(target = null)
                {
                    state = stateMachine.roming;
                }
                break;
                
            
            case stateMachine.Chase:
                leafParticle.Stop();
                attacking = false;
                chaseTimer += Time.deltaTime;
                if (chaseTimer <= 2f)
                {
              
                        moveSpeed = 1f;
                        float positionX = targetLastSeen_posX - transform.position.x;
                        if (transform.position.x > positionX)
                        {
                            transform.localScale = new Vector2((Mathf.Sign(rb2d.velocity.x)), transform.localScale.y);
                        }

                        
                        if (Mathf.Abs(positionX) > 0.002)
                        {
                            rb2d.velocity = new Vector2(positionX, 0).normalized * moveSpeed;
                            animator.SetBool("IsMoving", true);
                            effects.didPlayRollDust = false;

                        }
  
                }
                else
                {
                    moveSpeed = -moveSpeed;
                    target = null;
                    hasSpotedPlayer = false;
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
            chaseTimer = 6;

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
    public void TurnToPlayer()
    {
        if (!isTurning && target == null)
        {
            // print(collision.tag);
            StartCoroutine(Turn());
            transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), transform.localScale.y);
        }
    }
    private void LookForTarget()
    {

        if (rb2d != null && health > 0)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position + offset, transform.TransformDirection(rb2d.velocity),
                eyes_Range, eyes_Layer);
            if (hit2D.collider != null )
            {
                
                if (hit2D.collider.CompareTag("Player") && !attacking && !isWiggleOn)
                {
                    // print("see");
                    hasSpotedPlayer = true;
                    RandomSec = Random.Range(wiggleMinTime, wiggleMaxTime);
                    target = hit2D.collider.gameObject;
                    enemySoundManager.PlayOneSound("Snore");
                    state = stateMachine.Wiggle;
                }
               
            }
            else if (hit2D.collider == null && !isFlying && !isWiggleOn  )
            {
                
                if (target != null && hasSpotedPlayer)
                {
                    animator.SetBool("isAttacking", false);
                    targetLastSeen_posX = target.transform.position.x;
                    state = stateMachine.Chase;
                    return;
                }
                chaseTimer = 0;
                moveSpeed = Mathf.Lerp(moveSpeed, 1f, 5f*Time.deltaTime);
                animator.speed = Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                animator.SetBool("isAttacking", false);
                

                state = stateMachine.roming;
               // print(animator.GetBool("isAttacking"));
                
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
   public void NeedForKick()
   {
        
        state = stateMachine.Kick;
   }

    private IEnumerator KickPlayer()
    {
        yield return new WaitForSeconds(1f);
        kicking = true;
        animator.SetBool("isKicking", true);
    }

    private void FaceTarget()
    {
        if(target != null)
        {
            
            if (transform.position.x > target.transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }
}
