using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermanController : MonoBehaviour
{
    [SerializeField] private Collider2D deathCollider;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;
    EnemyHealthBar enemyHealthBar;

    private float walkSpeed = 1.5f;

    private bool playOnce;
    private bool grounded;
    private bool attack;
    private bool walking;
    private bool isDead;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        deathCollider.enabled = false;
    }

    protected void Start()
    {
        animator.SetBool("isWalking", true);
    }

    protected void LateUpdate()
    {
        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            animator.SetTrigger("isDead");
            this.GetComponent<Collider2D>().enabled = false;
            deathCollider.enabled = true;
            wanderWalkController.enabled = false;
            hurtPlayerOnContact.enabled = false;
            hurtEnemyOnContact.enabled = false;
            enemyHealthBar.enabled = false;
            isDead = true;
            Destroy(gameObject, 6f);
        }
        else if (!isDead)
        {
            SetAnimationLogic();
        }
    }

    private void SetAnimationLogic()
    {
        if (hurtPlayerOnContact.attackingAnimation)
        {
            StartCoroutine(TongueAttack());
        }
        else
        {
            playOnce = false;
        }

        if (wanderWalkController.isRunning)
        {
            animator.SetFloat("walkingSpeed", 0.6f);
        }
        else if (!wanderWalkController.isRunning)
        {
            animator.SetFloat("walkingSpeed", 0.2f);
        }

        if (wanderWalkController.isWalking || wanderWalkController.isRunning)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator TongueAttack()
    {
        if (!playOnce)
        {
            animator.SetTrigger("prepareToAttack");
            playOnce = true;
            if (wanderWalkController.isRunning)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.3f);
            if (!wanderWalkController.isRunning)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}
