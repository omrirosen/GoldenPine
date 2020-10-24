using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck_Unihog : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Unihog1Controller unihog;
    [SerializeField] Unihog1DMG dMG;
    RaycastHit2D hit;
    public LayerMask raylayer;

    private void Awake()
    {
        unihog = GetComponent<Unihog1Controller>();
    }

    private void Update()
    {
        if (dMG != null)
        {
            if (dMG.isunderImpact)
            {
                if (unihog.IsFacingRight())
                {
                    chackRay(Vector3.right);
                }
                else
                {
                    chackRay(Vector3.left);
                }
            }
            else
            {
                chackRay(Vector3.right);
                Debug.DrawRay(startPos.position, startPos.TransformDirection(Vector3.right) * 0.5f, Color.red);
            }
        }
        else return;
        
    }

    void chackRay(Vector3 side)
    {
        hit = Physics2D.Raycast(startPos.position, startPos.TransformDirection(side), 0.5f, raylayer);
        if(hit.collider!=null)
        {
            if(hit.collider.CompareTag("TileMapCollider"))
            {
                StartCoroutine(unihog.Turn());
                transform.localScale = new Vector2(-(Mathf.Sign(unihog.rb2d.velocity.x)), transform.localScale.y);
            }
        }
    }
}
