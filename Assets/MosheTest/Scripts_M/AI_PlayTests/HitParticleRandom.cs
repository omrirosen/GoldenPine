using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleRandom : MonoBehaviour
{
    [SerializeField] Animator animator;
    public int index;

    private void Start()
    {
        animator = GetComponent<Animator>();
        index = GiveRandomInt();
        animator.SetInteger("Index", index);
        //print(index);
    }


    public int GiveRandomInt()
    {
        return Random.Range(-1, 3);
    }

    public void KillME()
    {
        Destroy(transform.parent.gameObject);
    }
}
