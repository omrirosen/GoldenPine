using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1DMG : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] Unihog1Controller unihog;
    [SerializeField] float force;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit");
            collision.GetComponent<PlayerStats>().TakeDmg(dmg);
            if(collision.GetComponent<PlayerStats>().ParryWindow)
            {
                print("Parry");
               if(unihog.IsFacingRight())
               {
                    unihog.rb2d.bodyType = RigidbodyType2D.Dynamic;
                    print("Right");
                    //unihog.rb2d.AddForceAtPosition(Vector2.left * force, unihog.transform.position,ForceMode2D.Impulse);
                   
                    Invoke("ResetBodyType", 0.1f);
               }
               else
               {
                    unihog.rb2d.bodyType = RigidbodyType2D.Dynamic;
                    unihog.rb2d.AddForce(Vector2.right*force , ForceMode2D.Impulse);
                    Invoke("ResetBodyType", 0.1f);
               }
            }
        }
    }

    private void ResetBodyType()
    {
        unihog.rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
}
