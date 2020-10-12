using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Animator animator;
    public bool isTurning = false;


    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > -Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (!isTurning)
        {
            StartCoroutine(Turn());
            transform.localScale = new Vector2(-(Mathf.Sign(rb2d.velocity.x)), transform.localScale.y);
        }
    }

    IEnumerator Turn()
    {
        isTurning = true;
        animator.SetBool("IsTurning", isTurning);
        yield return new WaitForSeconds(0.3f);  
        isTurning = false;
        animator.SetBool("IsTurning", isTurning);
    }

}
