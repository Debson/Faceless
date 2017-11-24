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

    Animator animator;
    AudioManager audioManager;
    WalkMovement walkMovement;

    public bool attackingAnimation { set; get; }

    public bool isInTrigger { set; get; }

    public bool isAttacking { set; get; }

    public bool isHurted { get; set; }

    private bool firstHit = true;

    private float delay;

    protected void Awake()
    {
        animator = GetComponentInParent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        walkMovement = FindObjectOfType<WalkMovement>();
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
            isInTrigger = true;
            Attack();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInTrigger = false;
        }
    }

    IEnumerator Animation()
    {
        // On first contact with player don't apply any attack delay
        if (!firstHit)
        {
            yield return new WaitForSeconds(attackFreq);
        }
        if (hitAfterTime)
        {
            yield return new WaitForSeconds(hitDelay);
        }

        if (isInTrigger)
        {
            attackingAnimation = true;
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            Invoke("AttackAnimation", animationDelay);
        }
        else
        {
            isAttacking = false;
            firstHit = true;
        }
    }

    private void AttackAnimation()
    {
        walkMovement.Knockback(this.gameObject);
        audioManager.playerHurt[Random.Range(0, 2)].Play();
        attackingAnimation = false;
        isAttacking = false;
        firstHit = false;
    }

    private void Attack()
    {
        isAttacking = true;
        StartCoroutine(Animation());
        return;
    }
}

