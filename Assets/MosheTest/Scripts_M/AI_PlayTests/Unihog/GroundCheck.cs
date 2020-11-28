using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float gravity = -9.8f;
    [SerializeField] Transform check_Pos;
    [SerializeField] float ray_Distance;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] LayerMask ground_Layer;
    [SerializeField] float originalY;
    [SerializeField] LayerMask doTweenLayer;
    [SerializeField] Vector2 offsetCricle;
    public bool onDoTweenLayer;
    private Unihog1Controller unihog;
    public float physicsCircleRadius;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        unihog = GetComponent<Unihog1Controller>();
    }
    

    private void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(check_Pos.position, Vector2.down, ray_Distance, ground_Layer);
        if (hit2D.collider == null)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravity);
        }
        else if(hit2D.collider == null && Mathf.Abs(originalY - transform.position.y) <= 1.5f)
        {
            transform.position = new Vector2(transform.position.x, originalY);
        }

        if(Mathf.Abs(originalY-transform.position.y)>1.5f)
        {
            if (hit2D.collider)
            {
                print("Fall to dead");
                unihog.health = 0;
            }
        }
        onDoTweenLayer = Physics2D.OverlapCircle((Vector2)transform.position + offsetCricle, physicsCircleRadius, doTweenLayer);
        //Debug.DrawRay(check_Pos.position, Vector2.down * ray_Distance, Color.black);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + offsetCricle, physicsCircleRadius);
    }

}
