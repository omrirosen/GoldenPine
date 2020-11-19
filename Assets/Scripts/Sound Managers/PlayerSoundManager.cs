using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;

public class PlayerSoundManager : MonoBehaviour
{
    public void PlayFootstepsSound()
    {
        AudioManager.PlaySound(Sounds.Footsteps);
    }

    public void PlayDashSound()
    {
       //AudioManager.PlaySound(Sounds.Dash);
    }

    public void PlayJumpSound()
    {
        //AudioManager.PlaySound(Sounds.Jump);
    }
}
