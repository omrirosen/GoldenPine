using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using JSAM;

public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 4;
    public float DashStock = 0;
    [SerializeField] private GameObject PlayerSelf;
    [SerializeField] private Animator healthAnimator;
    [SerializeField] private Animator staminaAnimator;
    [SerializeField] private SpriteRenderer sRenderer;
    public bool shieldOn = false;
    public bool ParryWindow = false;
    private Color OGcolor;
    public Color DashColor;
    public bool DashAttacked = false;
    private PlayerWithShield playerWithShield;
    [SerializeField] private GameObject Buddy;
    public bool HitSpikes = false;
    public float parrywindowTime =0.3f;
    private bool isImpect_ON = false;
    private Rigidbody2D rb2d;
    [SerializeField] float impact_Force;
    [SerializeField] float impact_JumpForce;
    private Tween Impact;
    public float sensetive_Parry = 0;
    [SerializeField] CollisionCheck collisionCheck;
    [SerializeField] GameObject ParryPopGround;
    [SerializeField] GameObject ParryPopAir;
    [SerializeField] Transform hitpoint;
    [SerializeField] Transform hitpoint1;
    [SerializeField] GameObject ShieldHit1;
    [SerializeField] ShieldBubble ShieldReff;
    public bool ShieldCoolDown = false;
    public bool DashAttackOn = false;
    bool isInvinsable = false;
    
    private void Awake()
    {
        OGcolor = sRenderer.color;
        playerWithShield = this.GetComponent<PlayerWithShield>();
        rb2d = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisionCheck>();
        
    }

    private void Start()
    {
        DashStock = 0f;
    }

    private void Update()
    {
        ShieldUp();
        Dashed();
        if (Impact != null)
        {
            if (!Impact.IsPlaying())
            {
                isImpect_ON = false;
            }
            else
            {
                if(collisionCheck.onDoTween)
                {
                    Impact.Kill();
                }
             
            }
                
            
        }

        if (DashStock > 1)
        {
            DashStock = 1f;
        }

        

        if (DashStock < 1)
        {
            JSAM.AudioManager.StopSoundLoop(Sounds.StaminaFull);
            JSAM.AudioManager.StopSound(Sounds.StaminaFull);
        }
    }

    public void TakeDmg(int Dmg,Vector3 dir)
    {
        if (shieldOn == false && ParryWindow == false  && HitSpikes == false && isInvinsable == false && playerWithShield.DashAttack == false)
        {
            StartCoroutine("Blinker");
            playerHealth -= Dmg;
            healthAnimator.SetInteger("PlayerHealthUI", playerHealth);
            playerWithShield.UnderImpactAnim();
            playerWithShield.isDashing = false;
            if (!isImpect_ON)
            {
                isImpect_ON = true;
                Impact = rb2d.DOJump(transform.position - dir * impact_Force, impact_JumpForce, 0, 0.5f);
                Impact.SetEase(Ease.Flash);
            }
        }
        if(HitSpikes == true)
        {
            playerHealth -= Dmg;
            healthAnimator.SetBool("OneShot", true);
        }

        if(shieldOn == true)
        {
            ShieldCoolDown = true;
            shieldOn = false;
            Invoke("SetShieldCoolTime", 1.5f);
            if (dir.x <= 0)
            {
                Instantiate(ShieldHit1, hitpoint.transform.position, transform.rotation);
            }
            if (dir.x >= 0)
            {
                Instantiate(ShieldHit1, hitpoint1.transform.position, transform.rotation);
            }
            ShieldReff.HitShield();
            playerWithShield.HitShield();
        }

       if ( ParryWindow == true) 
       {
            
            Parry();
       }

       if (playerHealth <= 0)
       {
            playerWithShield.PlayerDeath();
            Invoke("Die", 1f);
            
       }
    }

    

    private void Die()
    {
       
        PlayerSelf.SetActive(false);
    }

    /* public void HealPlayer()
     {
         if (Input.GetKeyDown(KeyCode.T))
         {
             if (playerHealth == 1)
             {
                 animator.Play("IncreaseHealth");
                 playerHealth = 4;
             }

             if(playerHealth == 2)
             {
                 animator.Play("IncreaseHealth_2");
                 playerHealth = 4;
             }

             if(playerHealth == 3)
             {
                 animator.Play("IncreaseHealth_3");
                 playerHealth = 4;
             }
             animator.SetInteger("PlayerHealthUI", playerHealth);
         }
     }*/

    private void ShieldUp()
    {
        if (playerWithShield.isDashing == false)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                shieldOn = true;
                Buddy.SetActive(false);
                ParryWindow = true;
                Invoke("ParryEnd", parrywindowTime);

            }
            /*if(Input.GetKey(KeyCode.X))
            {
                sensetive_Parry += Time.deltaTime;
            }
            if(sensetive_Parry > 3)
            {
                shieldOn = false;
                Buddy.SetActive(true);
            }*/
            if (Input.GetKeyUp(KeyCode.X))
            {
                shieldOn = false;
                Buddy.SetActive(true);

                /*if(sensetive_Parry<=3f)
                {


                    sensetive_Parry = 0f;
                }
                else
                {
                    sensetive_Parry = 0f;
                }*/


            }
        }
        if (playerWithShield.isDashing == true)
        {
            shieldOn = false;
            Buddy.SetActive(true);

        }
    }

    private void ParryEnd()
    {
        ParryWindow = false;
    }
    
    private void Parry()
    {
        JSAM.AudioManager.PlaySound(Sounds.Parry);
       
        if (DashStock < 1)
        {
            DashStock = DashStock +1;
            staminaAnimator.SetFloat("StaminaUi", DashStock);
            
        }

        if (collisionCheck.onGround)
        {
            Instantiate(ParryPopGround, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(ParryPopAir, transform.position, transform.rotation);
        }
        
    }


    private void Dashed()
    {
        if (DashStock >= 1 && DashAttackOn == true)
        {
            Buddy.SetActive(false);
            DashStock = DashStock - 1;
            staminaAnimator.SetFloat("StaminaUi", DashStock);
            staminaAnimator.Play("UsedStamina");
            playerWithShield.DashAttack = true;
            //playerWithShield.isDashing = true;
            DashAttacked = true;
            playerWithShield.GeneratPulse();
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            Invoke("ResetConditions", 0.5f);
            print("DashStok = " + DashStock);
        }
    }

    private void ResetConditions()
    {
        DashAttacked = false;
        playerWithShield.isDashing = false;
        Buddy.SetActive(true);
    }

    public void IncreaseStamina()
    {
        DashStock += 0.2f;
        staminaAnimator.SetFloat("StaminaUi", DashStock);
    }

    private void SetShieldCoolTime()
    {
         ShieldCoolDown = false;
    }

    IEnumerator Blinker()
    {
        Color tmp = sRenderer.color;
        sRenderer.color = tmp;
        isInvinsable = true;

        sRenderer.color = tmp;
        tmp.a = 255;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.a = 0;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.a = 255f;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;
        tmp.a = 0;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 155f;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 0;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 55f;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 0;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 25f;

        yield return new WaitForSeconds(0.10f);
        sRenderer.color = tmp;

        tmp.a = 0;
        isInvinsable = false;
        StopCoroutine("Blinker");
    }
}
