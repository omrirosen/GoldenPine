using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSAM;
public class DestroyParryPop : MonoBehaviour
{
    public GameObject BlackDropInstPoint;
    private BoxCollider2D myCollider2D;
    private PlayerWithShield playerSctript;
    private SoundManager soundManager;
    private void Awake()
    {
        myCollider2D = GetComponent<BoxCollider2D>();
        playerSctript = FindObjectOfType<PlayerWithShield>();
        soundManager = FindObjectOfType<SoundManager>();
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

    public void CanPierceDash()
    {
        playerSctript.canPierceDash = true;
    }   

    public void PlayChargeSound()
    {
       // JSAM.AudioManager.PlaySound(Sounds.ChargeAnim);
       soundManager.PlayOneSound("Charge Anim");
    }

    public void StaminaFullSound()
    {
       // JSAM.AudioManager.PlaySoundLoop(Sounds.StaminaFull);
       soundManager.PlayOneSound("Stamina Full");
    }
}
