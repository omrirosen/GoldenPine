using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnihogGettingHit : MonoBehaviour
{
    [SerializeField] Unihog1Controller unihog;
    public void killMe(int dmg)
    {
        unihog.killme(dmg);
        
    }
}
