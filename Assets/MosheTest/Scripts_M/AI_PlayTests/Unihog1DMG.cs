using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unihog1DMG : MonoBehaviour
{
    [SerializeField] float dmg;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit");
            //TODO dmg the player
        }
    }
}
