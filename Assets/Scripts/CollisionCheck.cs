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

    [SerializeField] private float wallCollisionRadius = 0.25f;
    [SerializeField] private float bottomCollisionRadius = 0.25f;

    public Vector2 bottomLeftOffset, bottomRightOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;
    
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomLeftOffset, bottomCollisionRadius, groundLayer)
                  || Physics2D.OverlapCircle((Vector2)transform.position + bottomRightOffset, bottomCollisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, wallCollisionRadius, wallLayer) 
                 || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, wallCollisionRadius, wallLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, wallCollisionRadius, wallLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, wallCollisionRadius, wallLayer);

        wallSide = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomLeftOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomLeftOffset, bottomCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, wallCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, wallCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomRightOffset, bottomCollisionRadius);
    }
}
