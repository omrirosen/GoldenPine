using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck_Unihog : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Unihog1Controller unihog;
    RaycastHit2D hit;
    public LayerMask raylayer;

    private void Awake()
    {
        unihog = GetComponent<Unihog1Controller>();
    }

    private void Update()
    {
        chackRay();
        Debug.DrawRay(startPos.position, startPos.TransformDirection(Vector3.right) * 0.5f,Color.red);
    }

    void chackRay()
    {
        hit = Physics2D.Raycast(startPos.position, startPos.TransformDirection(Vector3.right), 0.5f, raylayer);
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
