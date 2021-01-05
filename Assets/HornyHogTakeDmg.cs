using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogTakeDmg : MonoBehaviour
{
    [SerializeField] HornyHogController hornyHogController;
    [SerializeField] SpriteRenderer sRenderer;
    [SerializeField] Color ogColor;
    private bool takenHit = false;
 

    private void Start()
    {
        ogColor = sRenderer.color;
    }
   
    public void TakeDMG(int dmg)
    {
        if (!takenHit)
        {
            hornyHogController.TakeDMG(dmg);
            takenHit = true;
            StartCoroutine("FlashRed");
            Invoke("SetTakeHitFlase", 0.2f);
        }
        
    }

    private void SetTakeHitFlase()
    {
        takenHit = false;
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

