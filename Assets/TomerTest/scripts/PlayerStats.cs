using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 4;
    private float DashStock = 0;
    [SerializeField] private GameObject PlayerSelf;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator StaminaAnimator;
    [SerializeField] private SpriteRenderer sRenderer;
    public bool shieldOn = false;
    public bool ParryWindow = false;
    private Color OGcolor;
    public Color DashColor;
    public bool DashAttacked = false;
    private PlayerWithShield playerWithShield;
    [SerializeField] private GameObject Buddy;


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
    bool ShieldCoolDown = false;

    private void Awake()
    {
        OGcolor = sRenderer.color;
        playerWithShield = this.GetComponent<PlayerWithShield>();
        rb2d = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisionCheck>();
    }
    private void Update()
    {
       // print(playerHealth);
        HealPlayer();
        ShieldUp();
        Dashed();
        print(ShieldCoolDown);
        if (Impact != null)
        {
            if (!Impact.IsPlaying())
            {
                isImpect_ON = false;
            }
            else
            {
                if(collisionCheck.onWall)
                {
                    Impact.Kill();
                }
            }
                
            
        }
    }

    public void TakeDmg(int Dmg,Vector3 dir)
    {
        if (ShieldCoolDown == true || shieldOn == false && ParryWindow == false && playerWithShield.isDashing == false)
        {
            playerHealth -= Dmg;
            animator.SetInteger("PlayerHealthUI", playerHealth);
            playerWithShield.UnderImpactAnim();
            if (!isImpect_ON)
            {
                isImpect_ON = true;
                Impact = rb2d.DOJump(transform.position - dir * impact_Force, impact_JumpForce, 0, 0.5f);
                Impact.SetEase(Ease.Flash);
            }
        }

        if(shieldOn == true && ShieldCoolDown == false)
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
            Invoke("Die", 1f);
            playerWithShield.PlayerDeath();
       }
    }

    

    private void Die()
    {
       
        PlayerSelf.SetActive(false);
    }
    
    public void HealPlayer()
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
    }

    private void ShieldUp()
    {
        if (Input.GetKey(KeyCode.X) && ShieldCoolDown == false)
        {

         shieldOn = true;  
         Buddy.SetActive(false);
         sensetive_Parry += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            shieldOn = false;            
            Buddy.SetActive(true);
            if(sensetive_Parry<=1f)
            {
                ParryWindow = true;
                Invoke("ParryEnd", 0.5f);
                sensetive_Parry = 0f;
            }
            else
            {
                sensetive_Parry = 0f;
            }
            

        }
    }

   
    
    private void ParryEnd()
    {
        ParryWindow = false;
    }
    
    private void Parry()
    {
       
        if (DashStock < 1)
        {
            DashStock = DashStock +1;
            StaminaAnimator.SetFloat("StaminaUi", DashStock);
            
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && DashStock >= 1)
        {
            DashStock = DashStock - 1;
            StaminaAnimator.SetFloat("StaminaUi", DashStock);
            StaminaAnimator.Play("StaminaDown");
            DashAttacked = true;
            playerWithShield.DashAttack = true;
            playerWithShield.isDashing = true;
            playerWithShield.GeneratPulse();
            FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            Invoke("BackToOGColor", 0.5f); 
        }
    }

    private void BackToOGColor()
    {
        DashAttacked = false;
        playerWithShield.isDashing = false;
    }

    public void IncreaseStamina()
    {
        DashStock += 0.2f;
        StaminaAnimator.SetFloat("StaminaUi", DashStock);
        print(DashStock);
    }

    private void SetShieldCoolTime()
    {
         ShieldCoolDown = false;
    }
}
