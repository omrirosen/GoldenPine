using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogTakeDmg : MonoBehaviour
{
    [SerializeField] HornyHogController hornyHogController;
    public void TakeDMG(int dmg)
    {
        hornyHogController.TakeDMG(dmg);
    }
}
