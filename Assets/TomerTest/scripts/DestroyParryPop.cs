using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParryPop : MonoBehaviour
{
    public GameObject BlackDropInstPoint;
    public void DieShiled(GameObject popPrefab)
    {
        Destroy(gameObject);
    }

    public void SetFals (GameObject ChargPrefab)
    {
        gameObject.SetActive(false);
    }

    public void SetTrue(GameObject DropPrefab)
    {
        print("im here");
        BlackDropInstPoint = GameObject.FindGameObjectWithTag("test");
        Instantiate(gameObject, BlackDropInstPoint.transform.position, BlackDropInstPoint.transform.rotation);
    }
}
