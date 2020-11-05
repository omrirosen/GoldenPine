using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParryPop : MonoBehaviour
{

    public void DieShiled(GameObject popPrefab)
    {
        Destroy(gameObject);
    }

    public void SetFals (GameObject ChargPrefab)
    {
        gameObject.SetActive(false);
    }
}
