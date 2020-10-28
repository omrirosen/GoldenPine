using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects :MonoBehaviour
{
    [SerializeField] GameObject UnihogRollDust_ins;
    [SerializeField] Transform UnihogRollDust_position;
    [SerializeField] Unihog1Controller unihog;
    public bool didPlayRollDust = false;

    public void playRollDust()
    {
        print("Dust");
        if (!didPlayRollDust)
        {
            didPlayRollDust = true;
            if (unihog.IsFacingRight())
            {
                GameObject temp = Instantiate(UnihogRollDust_ins, UnihogRollDust_position.position, Quaternion.identity);
                temp.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                GameObject temp = Instantiate(UnihogRollDust_ins, UnihogRollDust_position.position, Quaternion.identity);
                temp.GetComponentInChildren<SpriteRenderer>().flipX = false;
            }

        }
        
    }

   


}
