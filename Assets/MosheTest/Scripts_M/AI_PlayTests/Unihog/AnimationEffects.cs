using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects :MonoBehaviour
{
    [SerializeField] GameObject UnihogRollDust_ins;
    [SerializeField] Transform UnihogRollDust_position;
    public bool didPlayRollDust = false;

    public void playRollDust()
    {
        if (!didPlayRollDust)
        {
            didPlayRollDust = true;
            GameObject temp = Instantiate(UnihogRollDust_ins, UnihogRollDust_position.position, Quaternion.identity);
            
        }
    }

   


}
