using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;
using Random = System.Random;

public class BuddyController : MonoBehaviour
{
    private GameObject Self;
    [SerializeField] private Animator Anim;
    private SpriteRenderer SRenderer;
    private Collider2D AttackColl;
    bool IsJumping = false;
    bool JustAttacked = false;
    int NumOfClicks = 0;
    float LastClickedTime = 0f;
    [SerializeField] private float MaxComboDelay = 0.3f;
    [SerializeField] private float zCooldownTimer = 0.3f;
    [SerializeField] private Transform AttackTransform;
    [SerializeField]private Transform OGTransform;
    [SerializeField]float Speed = 1f;
    float MoveSpeed;
    [SerializeField] PlayerStats playerstats;
    public Vector2 Offset;
    public LayerMask AIlayer;
    public Vector2 OffsetLeft;
    public Vector2 OffsetRight;
    private bool zCooldown = false;
    private PlayerStats PS;
    public bool isBuddyFlipped = false;
    private CollisionCheck playerCollisionCheck;
    bool movedToAttackPoint = false;
    [SerializeField] private SoundManager soundManager;
    public bool playerIsDead = false;
    [SerializeField] GameObject EnemyParticle;
    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        Anim = GetComponent<Animator>();
        Self = this.gameObject;
        AttackColl = GetComponent<BoxCollider2D>();
        SRenderer = GetComponent<SpriteRenderer>();
        PS = GetComponentInParent<PlayerStats>();
        playerCollisionCheck = GetComponentInParent<CollisionCheck>();
        
    }

    private void Update()
    {
        if (!playerIsDead)
        {
            flipBuddy();
            Attack();
            MoveSpeed = Speed * Time.deltaTime;
        }
        Anim.SetInteger("numbOfClicks", NumOfClicks);
    }

    public void flip()
    {
        Anim.Play("Buddy_flip");
    }

    public void Dash()
    {
        Anim.Play("Buddy_dash");
    }

    public void Run()
    {
       
        Anim.SetBool("IsMoving", true);
        
    }

    public void StopRun()
    {
        Anim.SetBool("IsMoving", false);
    }
   
    public void Jump()
    {
        Anim.SetBool("IsJumping", true);
        IsJumping = true;
    }

    public void StopJump()
    {
        Anim.SetBool("IsJumping", false);
        IsJumping = false;
    }

    private void Attack()
    {
        if(Time.time - LastClickedTime > MaxComboDelay)
        {
            NumOfClicks = 0;
            
        }

        if (Input.GetKeyDown(KeyCode.Z) && !zCooldown && PS.DashStock < 1)
        {
          
            LastClickedTime = Time.time;
            NumOfClicks++;
            print(NumOfClicks);
            
            SRenderer.sortingOrder = 10;
            Invoke("ResetSortingOrder", 0.7f);
            if(NumOfClicks == 1 && !zCooldown)
            {
                print("enter 1st att");
                Anim.SetBool("Attack1", true);
                MoveToAttackPoint();
                soundManager.PlayzButtonArray();
                Invoke("SetAttack1False", 0.1f);
                Collider2D Enemy = Physics2D.OverlapCircle((Vector2)transform.position , 0.25f, AIlayer);//transform pos had +offset before. NEED TO CHECK IF ANYTHING CHANGED WITH ENEMY INTERACTION!!!!
                if (Enemy != null)
                {
                    soundManager.PlayOneSound("Z Absorbtion");
                    playerstats.IncreaseStamina();
                    /*
                    GameObject extractWhite =  Instantiate(EnemyParticle, AttackTransform.position, transform.rotation);
                    float randomScale;
                    randomScale = UnityEngine.Random.Range(0.3f, 0.7f);
                    extractWhite.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    */
                    if (Enemy.GetComponent<DeadUniHogTut>())
                    {
                        FindObjectOfType<DeadUniHogTut>().numOfHitTaken++;
                    }
                }
                
            }
            

            if (NumOfClicks == 2 && !zCooldown)
            {
                print("enter 2nd att");
                MoveToAttackPoint();
                Anim.SetBool("Attack1", false);
                Anim.SetBool("Attack2", true);
                soundManager.PlayzButtonArray();
                Collider2D Enemy = Physics2D.OverlapCircle((Vector2)transform.position, 0.25f, AIlayer);//transform pos had +offset before. NEED TO CHECK IF ANYTHING CHANGED WITH ENEMY INTERACTION!!!!
                Invoke("SetAttack2False", 0.1f);

                if(Enemy != null)
                {
                    soundManager.PlayOneSound("Z Absorbtion");
                    playerstats.IncreaseStamina();
                    /*
                   GameObject extractWhite =  Instantiate(EnemyParticle, AttackTransform.position, transform.rotation);
                   float randomScale;
                   randomScale = UnityEngine.Random.Range(0.3f, 0.7f);
                   extractWhite.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                   */
                }
            }
            if(NumOfClicks == 3 && !zCooldown)
            {
                print("entered 3rd attack");
                MoveToAttackPoint();
                Anim.SetBool("Attack1", true);
                soundManager.PlayzButtonArray();
                Invoke("SetAttack1False", 0.1f);
                Collider2D Enemy = Physics2D.OverlapCircle((Vector2)transform.position , 0.25f, AIlayer);//transform pos had +offset before. NEED TO CHECK IF ANYTHING CHANGED WITH ENEMY INTERACTION!!!!
                zCooldown = true;
                Invoke("SetZCooldownToFalse", zCooldownTimer);
                
                if (Enemy != null)
                {
                    soundManager.PlayOneSound("Z Absorbtion");
                    playerstats.IncreaseStamina();
                    /*
                   GameObject extractWhite =  Instantiate(EnemyParticle, AttackTransform.position, transform.rotation);
                   float randomScale;
                   randomScale = UnityEngine.Random.Range(0.3f, 0.7f);
                   extractWhite.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                   */
                    if (Enemy.GetComponent<DeadUniHogTut>())
                    {
                        print("takenhit");
                        FindObjectOfType<DeadUniHogTut>().numOfHitTaken++;
                    }
                }
                
            }
        }

        if(NumOfClicks == 0)
        {
            Anim.SetBool("Attack1", false);
            Anim.SetBool("Attack2", false);
            BackToOGPos();
        }
        NumOfClicks = Mathf.Clamp(NumOfClicks, 0, 2);
    }

    private void SetAttack1False()
    {
        Anim.SetBool("Attack1", false);
    }
    
    private void SetAttack2False()
    {
        Anim.SetBool("Attack2", false);
    }

    private void MoveToAttackPoint()
    {
        //if (!movedToAttackPoint)
        //{
            Self.transform.position = AttackTransform.position;
            movedToAttackPoint = true;
       // }
    }

    private void BackToOGPos()
    {
        
        Self.transform.position = Vector3.MoveTowards(Self.transform.position, OGTransform.position , MoveSpeed );
        if (Self.transform.position == OGTransform.position)
        {
            movedToAttackPoint = false;
        }
    }

    private void ResetSortingOrder()
    {
        SRenderer.sortingOrder = 1;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + Offset, 0.25f);
    }

    public void SetHitOffset()
    {
        /*if(Offset == OffsetRight)
        {
            Offset = OffsetLeft;
        }

        if (Offset == OffsetLeft)
        {
            Offset = OffsetRight;
        }*/
    }

    private void SetZCooldownToFalse()
    {
        zCooldown = false;
    }

   public void flipBuddy()
   {
        if (playerCollisionCheck.onWall)
        {
            if (!isBuddyFlipped)
            {
               
                transform.Rotate(0, 180, 0);
                
                isBuddyFlipped = true;
                
            }
        }

        if (!playerCollisionCheck.onWall)
        {
            if (isBuddyFlipped)
            {
                
                transform.Rotate(0, 180, 0);
                
                isBuddyFlipped = false;
            }
        }
       

   }

    public void BuddyDie()
    {
        Anim.SetBool("IsJumping", false);
        Anim.SetBool("IsMoving", false);
        Anim.SetBool("Attack1", false);
        Anim.SetBool("Attack2", false);
        Anim.Play("Buddy_Die");
    }
    
}



