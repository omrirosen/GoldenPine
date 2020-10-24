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
    private Transform target;
    private bool isDeath = false;
    private float deathTime = 0;
    public enum StateMachine { Idle,Attack,Death};
    StateMachine state;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = StateMachine.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
        StateMachineControll();
        if (health <= 0 && !isDeath)
        {
            isDeath = true;
            state = StateMachine.Death;
        }
        
    }

    public void StateMachineControll()
    {
        switch (state)
        {
            case StateMachine.Idle:

                IdleState();

                break;
            case StateMachine.Attack:

                AttackState();

                break;
            case StateMachine.Death:

                DeathState();

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

    public void AttackState()
    {
        animator.SetBool("IsAttacking", true);
       
        hogDMG.IsFachingRight = IsFachingRight();
        if (!CanAttack(target))
        {
            animator.SetBool("IsAttacking", false);
            hogDMG.isActive = false;
            state = StateMachine.Idle;
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
        else if(deathTime > 0.7f && deathTime < 10f)
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
        }
      //  print("TakeDMG");
       
    }
}
