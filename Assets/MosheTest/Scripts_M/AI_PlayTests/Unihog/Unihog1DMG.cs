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
    public bool isunderImpact = false;
    private Tween impact;
    private bool attack = false;
    [SerializeField]private GroundCheck groundCheck;
    private void Update()
    {
       
        
        if (impact!=null)
        {
            if (groundCheck.onDoTweenLayer)
            {
                impact.Kill();
                isunderImpact = false;
                unihog.isFlying = false;
            }
             if(!impact.IsPlaying())
            {
                isunderImpact = false;
                unihog.isFlying = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (unihog.attacking)
        {
            if (collision.gameObject.tag == "Player")
            {

                // print("hit");
                if (collision.GetComponent<PlayerStats>() != null)
                {
                    if (!attack)
                    {
                        if (unihog.IsFacingRight())
                        {
                            attack = true;
                            collision.GetComponent<PlayerStats>().TakeDmg(dmg, Vector3.left);
                            Invoke("resetAttack", 0.5f);
                            impact = unihog.rb2d.DOJump((transform.position - Vector3.right * force), jumpForce, 0, 0.5f);
                            impact.SetEase(Ease.Flash);
                            impact.SetUpdate(UpdateType.Fixed);
                            isunderImpact = true;
                            unihog.isFlying = true;
                        }
                        else if (!unihog.IsFacingRight())
                        {
                            attack = true;
                            collision.GetComponent<PlayerStats>().TakeDmg(dmg, Vector3.right);
                            Invoke("resetAttack", 0.5f);
                            impact = unihog.rb2d.DOJump((transform.position - Vector3.left * force), jumpForce, 0, 0.5f);
                            impact.SetEase(Ease.Flash);
                            impact.SetUpdate(UpdateType.Fixed);
                            isunderImpact = true;
                            unihog.isFlying = true;
                        }

                    }

                    if (collision.GetComponent<PlayerStats>().ParryWindow)
                    {
                        if (!isunderImpact)
                        {
                            // print("Parry");
                            if (unihog.IsFacingRight())
                            {

                                // print("Right");
                                impact = unihog.rb2d.DOJump((transform.position - Vector3.right * force), jumpForce, 0, 0.5f);
                                impact.SetEase(Ease.Flash);
                                impact.SetUpdate(UpdateType.Fixed);
                                isunderImpact = true;
                                unihog.isFlying = true;

                            }
                            else if (!unihog.IsFacingRight())
                            {
                                // print("left");
                                impact = unihog.rb2d.DOJump((transform.position - Vector3.left * force), jumpForce, 0, 0.5f);
                                impact.SetEase(Ease.Flash);
                                impact.SetUpdate(UpdateType.Fixed);
                                isunderImpact = true;
                                unihog.isFlying = true;

                            }
                        }

                    }


                }
            }
        }
    }


    public void resetAttack()
    {
        attack = false;
    }


   
}
