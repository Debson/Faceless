using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private int minDamageToGive;

    [SerializeField]
    private int maxDamageToGive;

    [SerializeField]
    private float attackFreqConst;

    [SerializeField]
    private float animationDelayConst = 0.55f;

    [SerializeField]
    private bool hitAfterTime;


    Animator animator;

    public bool isHurted
    { get; set;}


    [HideInInspector]
    public float hitTimer = 0.5f;

    [HideInInspector]
    public float hitDelay;

    [HideInInspector]
    public bool attackingAnimation;

    [HideInInspector]
    public bool isInTrigger;

    private float animationDelay;
    private float attackFreq;
    private bool isAttacking;
    


    protected void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    protected void Start()
    {
        attackFreq = attackFreqConst;
        animationDelay = animationDelayConst;
        hitDelay = hitTimer;

    }

    protected void Update()
    {
        if (isAttacking)
        {
            animationDelay -= Time.deltaTime;
            attackFreq -= Time.deltaTime;

            if(animationDelay < 0)
            {
                attackingAnimation = false;
                //animator.SetBool("isAttacking", false);
                //animator.SetBool("isInTrigger", true);
                isInTrigger = true;
            }

            if(attackFreq < 0)
            {
                isAttacking = false;
                attackingAnimation = false;
                //animator.SetBool("isAttacking", isAttacking);
                attackFreq = attackFreqConst;
                animationDelay = animationDelayConst;
                //animator.SetBool("isInTrigger", false);
                isInTrigger = false;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && hitAfterTime && !isHurted)
        {// Attack player 0.5s after entering AttackTrigger collider
            if(hitDelay > 0)
            {
                hitDelay -= Time.deltaTime;
                return;
            }
            isHurted = true;
        }
        else if (collision.tag == "Player" && !isAttacking)
        {
                isAttacking = true;
                attackingAnimation = true;
                HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
                //animator.SetBool("isAttacking", true);

                var player = collision.GetComponent<WalkMovement>();
                player.knockbackTimeCount = player.knockBackLength;

                if (collision.transform.position.x < transform.position.x)
                {
                    player.knockFromRight = true;
                }
                else
                {
                    player.knockFromRight = false;
                }
        }
    }
 }

