using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, SecndBotoom;
    private Color debugCollisionColor = Color.red;
    
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer) 
                  || Physics2D.OverlapCircle((Vector2)transform.position + SecndBotoom, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, wallLayer) 
                 || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, wallLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, wallLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, wallLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + SecndBotoom, collisionRadius);
    }
}
