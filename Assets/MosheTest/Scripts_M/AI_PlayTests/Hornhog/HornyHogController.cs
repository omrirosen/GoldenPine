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
    private bool isAttacking;
    
    //Circle Config
    [SerializeField] private float circleRadius;
    [SerializeField] private float closeRangeRadius;




    public enum StateMachine { Idle,Attack,Death,Chase};
    public StateMachine state;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = StateMachine.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        LookForPlayer();
        FaceTarget();
        StateMachineControll();
        if (health <= 0 && !isDeath)
        {
            isDeath = true;
            state = StateMachine.Death;
        }
        print(state);
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

    public void LookForPlayer()
    {
        if (!isAttacking)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, circleRadius, eyes_Layer);
            if (player != null)
            {
                target = player.transform;
                state = StateMachine.Chase;
            }
        }
        
        
    }

    public void Chase()
    {
        isAttacking = false;
        moveSpeed = 1f;
        float positionX = target.position.x - transform.position.x;
        if (transform.position.x > positionX)
        {
            transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
        if (Mathf.Abs(positionX) > 0.2)
        {
            rb.velocity = new Vector2(positionX, 0).normalized * moveSpeed;
        }
        
        
        Collider2D playerInCloseRange = Physics2D.OverlapCircle(transform.position, closeRangeRadius, eyes_Layer);
        if (playerInCloseRange)
        {
            if (CanAttack(playerInCloseRange.transform))
            {
                state = StateMachine.Attack;
            }
            
        }
        
    }

    public void AttackState()
    {
        moveSpeed = 0;
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        hogDMG.IsFachingRight = IsFachingRight();
        if (!CanAttack(target))
        {
            animator.SetBool("IsAttacking", false);
            hogDMG.isActive = false;
            state = StateMachine.Chase;
        }
        
    }

    public void DeathState()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDeath", true);
        if(deathTime<=0.7f)
        {
            deathTime += Time.deltaTime;
        }
        else if(deathTime > 0.7f && deathTime < 8f)
        {
            deathTime += Time.deltaTime;
            animator.SetFloat("DeathTime", 0.8f);
        }
        
        else if(deathTime >=10f)
        {
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
        print("TakeDMG");
       
    }
    
    private void FaceTarget()
    {
        if(target != null)
        {
            
            if (transform.position.x > target.transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                
                transform.localScale = new Vector2(-1, 1);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position, closeRangeRadius);
        Gizmos.DrawWireSphere((Vector2) transform.position, circleRadius);
    }
 
}
