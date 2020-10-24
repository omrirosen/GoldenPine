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

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    

    private void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(check_Pos.position, Vector2.down, ray_Distance, ground_Layer);
        if (hit2D.collider == null)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, gravity);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, originalY);
        }
        //Debug.DrawRay(check_Pos.position, Vector2.down * ray_Distance, Color.black);
    }
}
