using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogController : MonoBehaviour
{
    [SerializeField] float eyes_Range;
    [SerializeField] Animator animator;
    [SerializeField] Transform eyes_Transform;
    [SerializeField] LayerMask eyes_Layer;
    [SerializeField] HornyHogDMG hogDMG;
    [SerializeField] int health;
    [SerializeField] GameObject HitPartical_ins;
    private Rigidbody2D rb;
    private Transform target;
    private bool isDeath = false;
    private float deathTime = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float []ogMoveSpeed;
    public bool isAttacking;
    private EnemySpawner enemySpawner;
    private bool scoreCalculated = false;
    [SerializeField] private float randomAttack;
    private bool choseAttack;
    
    
    //Circle Config
    [SerializeField] private float circleRadius;
    [SerializeField] private float closeRangeRadius;




    public enum StateMachine { Idle,Attack,Death,Chase};
    public StateMachine state;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        snapToCorrectY();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = StateMachine.Chase;
        enemySpawner.numbOfEnemies++;
        moveSpeed = ogMoveSpeed[Random.Range(0, ogMoveSpeed.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeath)
        {
            LookForPlayer();
            FaceTarget();
        }
        StateMachineControll();
        if (health <= 0 && !isDeath)
        {
            isDeath = true;
            state = StateMachine.Death;
        }
        print(randomAttack + "random Attack");
        
    }

    public void StateMachineControll()
    {
        switch (state)
        {
            case StateMachine.Idle:

                //IdleState();

                break;
            case StateMachine.Attack:

                AttackState();

                break;
            case StateMachine.Death:

                DeathState();

                break;
            case StateMachine.Chase:
                Chase();
                
            break;
           
        }
    }

/*
    public void IdleState()
    {
        animator.SetBool("IsAttacking", false);
        if(IsFachingRight())
        {
            RaycastHit2D hit = Physics2D.Raycast(eyes_Transform.position, transform.TransformDirection(Vector2.right),
                eyes_Range, eyes_Layer);
            Debug.DrawRay(eyes_Transform.position, transform.TransformDirection(Vector2.right) * eyes_Range, Color.blue);
            if(hit.collider!=null)
            {
                // print("I see You");
              if( CanAttack(hit.transform))
                {
                    target = hit.transform;
                    state = StateMachine.Attack;
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(eyes_Transform.position, transform.TransformDirection(Vector2.left),
               eyes_Range, eyes_Layer);
            Debug.DrawRay(eyes_Transform.position, transform.TransformDirection(Vector2.left) * eyes_Range, Color.blue);
            if (hit.collider != null)
            {
                //  print("I see You");
                if (CanAttack(hit.transform))
                {
                    target = hit.transform;
                    state = StateMachine.Attack;
                }
            }
        }
    }
    */

    public void LookForPlayer()
    {
        if (!isAttacking)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, circleRadius, eyes_Layer);
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    public void Chase()
    {
        
        isAttacking = false;
        animator.SetBool("IsMoving", true);
        if (target != null)
        {
            float positionX = target.position.x - transform.position.x;
            if (Mathf.Abs(positionX) > 0.2f)
            {
                rb.velocity = new Vector2(positionX, 0).normalized * moveSpeed;
            }
        
            if(IsFachingRight())
            {
                RaycastHit2D hit = Physics2D.Raycast(eyes_Transform.position, transform.TransformDirection(Vector2.right),
                    eyes_Range, eyes_Layer);
                Debug.DrawRay(eyes_Transform.position, transform.TransformDirection(Vector2.right) * eyes_Range, Color.blue);
                if(hit.collider!=null)
                {
                    // print("I see You");
                    if( CanAttack(hit.transform))
                    {
                        target = hit.transform;
                        state = StateMachine.Attack;
                    }
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(eyes_Transform.position, transform.TransformDirection(Vector2.left),
                    eyes_Range, eyes_Layer);
                Debug.DrawRay(eyes_Transform.position, transform.TransformDirection(Vector2.left) * eyes_Range, Color.blue);
                if (hit.collider != null)
                {
                    //  print("I see You");
                    if (CanAttack(hit.transform))
                    {
                        target = hit.transform;
                        state = StateMachine.Attack;
                    }
                }
            }
        }
        else
        {
            state = StateMachine.Chase;
        }
    }

    public void AttackState()
    {
        rb.velocity = Vector2.zero;
        randomAttack = Random.Range(0,2);
        if (randomAttack <= 1)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsAttacking", true);
            hogDMG.IsFachingRight = IsFachingRight();
            if (!CanAttack(target))
            {
                animator.SetBool("IsAttacking", false);
                state = StateMachine.Chase;
            }
        }
        else if(randomAttack > 1)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsSwingAttack", true);
            hogDMG.IsFachingRight = IsFachingRight();
            if (!CanAttack(target))
            {
                animator.SetBool("IsSwingAttack", false);
                state = StateMachine.Chase;
            }
        }
       
        
    }

    public void DeathState()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDeath", true);
        animator.SetBool("IsMoving", false);
        if (!scoreCalculated)
        {
            enemySpawner.IncreasScore();
            scoreCalculated = true;
        }
        if(deathTime<=0.7f)
        {
            deathTime += Time.deltaTime;
        }
        if(deathTime > 0.7f && deathTime < 8f)
        {
            deathTime += Time.deltaTime;
            animator.SetFloat("DeathTime", 0.8f);
        }
         if (deathTime >= 3f)
        {
            animator.SetBool("Evaporate", true);
        }
        if(deathTime >= 3.8f)
        {
            if (enemySpawner != null)
            {
                enemySpawner.numbOfEnemies--;
                enemySpawner.enemiesDefeated++;
            }

            Destroy(gameObject);
        }

    }

    public bool IsFachingRight()
    {
        if (transform.localScale.x < 0)
        {
            return true;
        }
        else return false;
    }

    public bool CanAttack(Transform target)
    {
        if (Mathf.Abs((target.position - transform.position).magnitude) < 1f)
        {
            return true;
        }
        else return false;
    }

    public void TakeDMG(int dmg)
    {
        if (!isDeath)
        {
            health -= dmg;
            var temp = Instantiate(HitPartical_ins, transform.position, Quaternion.identity);
        }

       
    }
    
    private void FaceTarget()
    {
        if(target != null)
        {

            if (transform.position.x < target.transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position, closeRangeRadius);
        Gizmos.DrawWireSphere((Vector2) transform.position, circleRadius);
    }

    private void snapToCorrectY()
    {
        transform.position = new Vector2(transform.position.x, 0.363f);
    }
 
}
