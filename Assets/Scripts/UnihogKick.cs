using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnihogKick : MonoBehaviour
{
    Unihog1Controller unihog;
    [SerializeField] int dmg;
    [SerializeField] float force;
    [SerializeField] float jumpForce;
    public bool isunderImpact = false;
    private bool attack = false;

    private void Awake()
    {
        unihog = GetComponentInParent<Unihog1Controller>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {


            if (unihog.kicking)
            {
                if (collision.gameObject.tag == "Player")
                {

                    // print("hit");
                    if (collision.GetComponent<PlayerStats>() != null)
                    {
                    print("detected player");
                        if (!attack)
                        {
                            print("attack");
                            if (unihog.IsFacingRight())
                            {
                                print("excuted attack");
                                attack = true;
                                collision.GetComponent<PlayerStats>().TakeDmg(dmg, Vector3.left);
                                unihog.kicking = false;
                                Invoke("resetAttack", 0.5f);
                                
                                isunderImpact = true;
                                

                            }
                            else if (!unihog.IsFacingRight())
                            {
                                attack = true;
                                collision.GetComponent<PlayerStats>().TakeDmg(dmg, Vector3.right);
                                Invoke("resetAttack", 0.5f);
                                unihog.kicking = false;
                                isunderImpact = true;


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
