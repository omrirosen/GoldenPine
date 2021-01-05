using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnihogGettingHit : MonoBehaviour
{

    [SerializeField] Unihog1Controller unihog;
    [SerializeField] SpriteRenderer sRenderer;

  
    public void killMe(int dmg)
    {
        StartCoroutine("FlashRed");
        unihog.killme(dmg);
        
    }
    private IEnumerator FlashRed()
    {


        Color tmp = sRenderer.color;
        sRenderer.color = tmp;


        sRenderer.color = tmp;
        tmp.r = 204;
        tmp.g = 132;
        tmp.b = 172;
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.r = 0;
        tmp.g = 0;
        tmp.b = 0;
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.r = 204;
        tmp.g = 132;
        tmp.b = 172;
        /*
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.r = 0;
        tmp.g = 0;
        tmp.b = 0;
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 204;
        tmp.g = 132;
        tmp.b = 172; 

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 0;
        tmp.g = 0;
        tmp.b = 0;
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 204;
        tmp.g = 132;
        tmp.b = 172;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 0;
        tmp.g = 0;
        tmp.b = 0;
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 204;
        tmp.g = 132;
        tmp.b = 172;
        */
        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.r = 0;
        tmp.g = 0;
        tmp.b = 0;
        StopCoroutine("Blinker");
    }
}