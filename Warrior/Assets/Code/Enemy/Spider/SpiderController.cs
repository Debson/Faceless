using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] Collider2D deathCollider;

    HurtPlayerOnContact hurtPlayerOnContact;
    EnemyHealthManager enemyHealthManager;
    FloorDetector floorDetector;
    Animator animator;
    WanderWalkController wanderWalkController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthBar enemyHealthBar;

    private bool onlyOnce;
    private bool isDead;


    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        floorDetector = FindObjectOfType<FloorDetector>();
        animator = GetComponent<Animator>();
        wanderWalkController = GetComponent<WanderWalkController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        deathCollider.enabled = false;
    }

    protected void LateUpdate()
    {
        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            OnDeath();
        }
        else if(!isDead)
        {
            SetAnimationLogic();
        }
    }

    private void OnDeath()
    {
        animator.SetTrigger("isDead");
        gameObject.GetComponent<Collider2D>().enabled = false;
        deathCollider.enabled = true;
        wanderWalkController.enabled = false;
        hurtPlayerOnContact.enabled = false;
        hurtEnemyOnContact.enabled = false;
        enemyHealthBar.enabled = false;

        audioManager.spiderChattering.Stop();
        Destroy(gameObject, 6f);
    }

    private void SetAnimationLogic()
    {
        animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
        animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);

        if (hurtPlayerOnContact.attackingAnimation && onlyOnce)
        {
            animator.SetBool("isAttacking", true);
            audioManager.spiderAttack.Play();
            onlyOnce = false;
        }
        else if (!hurtPlayerOnContact.attackingAnimation)
        {
            animator.SetBool("isAttacking", false);
            onlyOnce = true;
        }

        if (!wanderWalkController.isRunning)
        {
            animator.SetFloat("Speed", 0.6f);
            audioManager.spiderChattering.Play();
        }
        else
        {
            animator.SetFloat("Speed", 1.5f);
        }
    }
}
