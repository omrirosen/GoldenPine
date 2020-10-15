using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1DMG : MonoBehaviour
{
    [SerializeField] int dmg;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit");
            collision.GetComponent<PlayerStats>().TakeDmg(dmg);
          //  if(collision.GetComponent<PlayerStats>()
        }
    }
}
