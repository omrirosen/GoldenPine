using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDropFalling : MonoBehaviour
{
    private Animator DropAnim;
    [SerializeField] GameObject BlackDrop2nd;
    [SerializeField] Transform InstPoint;
    [SerializeField] GameObject BlackDropFrist;
    [SerializeField] Transform FirstInstpoint;
    private void Awake()
    {
        BlackDropFrist = GameObject.Find("BlackDropCieling");
        DropAnim = GetComponent<Animator>();
        
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        DropAnim.SetBool("HasCollided", true);
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<PlayerStats>().TakeDmg(0, Vector3.zero);
        }
    }

    public void CreateDrop()
    {
        Instantiate(BlackDrop2nd, InstPoint.transform.position, InstPoint.transform.rotation);
    }

    public void createTopDrop()
    {
        BlackDropFrist.SetActive(true);
    }
}
