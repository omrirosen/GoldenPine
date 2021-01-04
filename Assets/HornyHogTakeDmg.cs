using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogTakeDmg : MonoBehaviour
{
    [SerializeField] HornyHogController hornyHogController;
    private bool takenHit = false;
    public void TakeDMG(int dmg)
    {
        if (!takenHit)
        {
            hornyHogController.TakeDMG(dmg);
            takenHit = true;
            Invoke("SetTakeHitFlase", 0.2f);
        }
        
    }

    private void SetTakeHitFlase()
    {
        takenHit = false;
    }
}
