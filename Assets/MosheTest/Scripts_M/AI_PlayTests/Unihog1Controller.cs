using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1Controller : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Animator animator;
    [SerializeField] float eyes_Range ;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask eyes_Layer;
    [SerializeField] float max_Speed;
    
    public bool isTurning = false;
    public enum stateMachine { roming, attack };
    public stateMachine state;
    private GameObject target;
    


   public Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        state = stateMachine.roming;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        StateMachineControll();
        if(rb2d!=null )
        {
            
            Debug.DrawRay(transform.position + offset, (transform.TransformDirection(rb2d.velocity)).normalized*eyes_Range, Color.red);
        }
        LookForTarget();

    }

    private void StateMachineControll()
    {
        switch (state)
        {
            case stateMachine.roming:
                if (!isTurning)
                {
                    if (IsFacingRight())
                    {
                        rb2d.velocity = new Vector2(moveSpeed, 0);

                        animator.SetBool("IsMoving", true);

                    }
                    else
                    {
                        rb2d.velocity = new Vector2(-moveSpeed, 0);
                        animator.SetBool("IsMoving", true);
                    }
                }
                else
                {
                    rb2d.velocity = Vector2.zero;
                    animator.SetBool("IsMoving", false);
                }

                break;
            case stateMachine.attack:
                animator.SetBool("isAttacking", true);
                
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
            default:
                break;
        }
    }

    private void MaxRollSpeed(Rigidbody2D player)
    {
        if(player.velocity.magnitude< max_Speed)
        {
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
        
        if (!isTurning && collision.tag!="Player")
        {
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
        if (rb2d != null)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position + offset, transform.TransformDirection(rb2d.velocity),
                eyes_Range, eyes_Layer);
            if (hit2D.collider != null)
            {
                if (hit2D.collider.CompareTag("Player"))
                {
                    // print("see");
                    target = hit2D.collider.gameObject;
                    state = stateMachine.attack;
                }
               
            }
            else
            {
              //  print("CantSee");
                moveSpeed = Mathf.Lerp(moveSpeed, 1f, 5f*Time.deltaTime);
                animator.speed = Mathf.Lerp(moveSpeed, 1f, 5f * Time.deltaTime);
                target = null;
                animator.SetBool("isAttacking", false);
                state = stateMachine.roming;
            }
        }

    }


    public void killme()
    {
        Destroy(gameObject);
    }
}
