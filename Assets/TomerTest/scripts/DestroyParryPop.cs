using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParryPop : MonoBehaviour
{
    public GameObject BlackDropInstPoint;
    private BoxCollider2D myCollider2D;

    private void Awake()
    {
        myCollider2D = GetComponent<BoxCollider2D>();
    }

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

    public void DestroyCollider()
    {
        Destroy(myCollider2D);
    }
}
