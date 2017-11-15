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
    private float attackFreq;

    [SerializeField]
    private float animationDelay = 0.55f;

    [SerializeField]
    private bool hitAfterTime;

    [SerializeField]
    private float hitDelay;

    [HideInInspector]
    public bool attackingAnimation;

    [HideInInspector]
    public bool isInTrigger;

    [HideInInspector]
    public bool isAttacking;

    Animator animator;

    public bool isHurted
    { get; set; }

    private bool firstHit = true;

    protected void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    protected void Start()
    {

    }

    protected void Update()
    {

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isAttacking)
        {
                isInTrigger =  true;
                StartCoroutine(Attack(collision));
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInTrigger = false;
        }
    }

    // Not needed for now
    IEnumerator BeforeAttackDelay(float time)
    {
        Debug.Log(isHurted);
        yield return new WaitForSeconds(time);
        isHurted = true;
    }

    IEnumerator Attack(Collider2D collision)
    {
        isAttacking = true;
        StartCoroutine(Animation(collision));
        yield return null;
    }

    IEnumerator Animation(Collider2D collision)
    {
        // On first contact with player don't apply any attack delay
        if (!firstHit)
        {
            yield return new WaitForSeconds(attackFreq);
        }
        // If player in in attacking collider, start animation, damage player, do knockback, else stop attacking sequence(avoid playing animation when player run out from collider range)
        if (isInTrigger)
        {
            if (hitAfterTime)
            {
                yield return new WaitForSeconds(hitDelay);
            }
            attackingAnimation = true;
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            StartCoroutine(Knockback(collision));

            yield return new WaitForSeconds(animationDelay);
            attackingAnimation = false;

            isAttacking = false;
            firstHit = false;
        }
        else
        {
            isAttacking = false;
            firstHit = true;
        }
    }

    IEnumerator Knockback(Collider2D collision)
    {
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
        yield return null;
    }
 }

