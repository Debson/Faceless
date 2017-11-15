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
    private float animationDelay = 0.55f;

    [SerializeField]
    private bool hitAfterTime;


    Animator animator;

    public bool isHurted
    { get; set;}


    [SerializeField]
    private float hitDelay;

    [HideInInspector]
    public bool attackingAnimation;

    [HideInInspector]
    public bool isInTrigger;

    [HideInInspector]
    public bool isAttacking;

    private float attackFreq;
    private bool inCollider;
    private bool firstHit = true;

    protected void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    protected void Start()
    {
        attackFreq = attackFreqConst;
    }

    protected void Update()
    {
        Debug.Log(inCollider);
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isAttacking)
        {
                inCollider =  true;
                StartCoroutine(Attack(collision));

        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inCollider = false;
        }
    }

    IEnumerator BeforeAttackDelay(float time)
    {
        Debug.Log(isHurted);
        yield return new WaitForSeconds(time);
        isHurted = true;
    }

    IEnumerator Attack(Collider2D collision)
    {
       
        isInTrigger = true;
        isAttacking = true;
        StartCoroutine(Animation(collision));
        yield return null;
    }

    IEnumerator Animation(Collider2D collision)
    {

        if (!firstHit)
        {
            yield return new WaitForSeconds(1.5f);
        }

        if (inCollider)
        {
            isInTrigger = true;
            attackingAnimation = true;
            yield return new WaitForSeconds(0.55f);
            attackingAnimation = false;

            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            StartCoroutine(Knockback(collision));
            isAttacking = false;
            firstHit = false;
        }
        else
        {
            Debug.Log("wyszedl");
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

