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

    Animator animator;

    private float animationDelay;
    private float attackFreq;
    private bool isAttacking;
    private bool isInTrigger;
    private bool check;
    private bool check2;


    protected void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    protected void Start()
    {
        attackFreq = attackFreqConst;
        animationDelay = animationDelayConst;
    }

    protected void Update()
    {
        if (isAttacking)
        {
            animationDelay -= Time.deltaTime;
            attackFreq -= Time.deltaTime;

            if(animationDelay < 0)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isInTrigger", true);
                isInTrigger = true;
            }

            if(attackFreq < 0)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
                attackFreq = attackFreqConst;
                animationDelay = animationDelayConst;
                animator.SetBool("isInTrigger", false);
                isInTrigger = false;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isAttacking)
        {
            HealthManager.HurtPlayer(minDamageToGive, maxDamageToGive);
            isAttacking = true;
            animator.SetBool("isAttacking", true);

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

