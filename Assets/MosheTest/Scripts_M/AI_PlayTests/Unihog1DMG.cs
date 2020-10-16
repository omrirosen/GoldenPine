using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unihog1DMG : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] Unihog1Controller unihog;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    private bool isunderImpact = false;
    private Tween impact;

    private void Update()
    {
        if(impact!=null)
        {
            if(!impact.IsPlaying())
            {
                isunderImpact = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           // print("hit");
            collision.GetComponent<PlayerStats>().TakeDmg(dmg);
            if(collision.GetComponent<PlayerStats>().ParryWindow)
            {
                if (!isunderImpact)
                {
                    // print("Parry");
                    if (unihog.IsFacingRight())
                    {

                        print("Right");
                        impact = unihog.rb2d.DOJump((transform.position - Vector3.right * force), jumpForce, 0, 0.5f);
                        impact.SetEase(Ease.Flash);
                        isunderImpact = true;

                    }
                    else if (!unihog.IsFacingRight())
                    {
                        print("left");
                        impact = unihog.rb2d.DOJump((transform.position - Vector3.left * force), jumpForce, 0, 0.5f);
                        impact.SetEase(Ease.Flash);
                        isunderImpact = true;

                    }
                }
            }
        }
    }

    public void killMe()
    {
        Destroy(unihog.gameObject);
    }

   
}
