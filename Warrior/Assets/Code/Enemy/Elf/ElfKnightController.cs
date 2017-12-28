using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfKnightController : MonoBehaviour
{
    [SerializeField]
    private Collider2D deathCollider;

    EnemyHealthBar enemyHealthBar;
    PlayerController playerController;
    Animator animator;
    EnemyHealthManager enemyHealthManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    AudioManager audioManager;
    FloorDetector floorDetector;
    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;

    private float direction;

    private bool isFacingRight;
    private bool isDead;
    private bool playOnce;
    private bool playOnce2;

    protected void Awake()
    {
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        audioManager = FindObjectOfType<AudioManager>();
        floorDetector = FindObjectOfType<FloorDetector>();
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
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
        deathCollider.enabled = true;
        animator.SetTrigger("isDead");
        wanderWalkController.enabled = false;
        enemyHealthBar.enabled = false;
        hurtEnemyOnContact.enabled = false;
        hurtPlayerOnContact.enabled = false;
        audioManager.elfDeath[0].Play();
        gameObject.GetComponent<Collider2D>().enabled = false;
        isDead = true;
        Destroy(gameObject, 6f);
    }

    private void SetAnimationLogic()
    {
        if (hurtEnemyOnContact.isHurt && !playOnce)
        {
            animator.SetTrigger("isHurt");
            audioManager.elfHurt[Random.Range(0, 3)].Play();
            playOnce = true;
        }
        else if(!hurtEnemyOnContact.isHurt)
        {
            playOnce = false;
        }

        if(hurtPlayerOnContact.attackingAnimation && !playOnce2)
        {
            audioManager.elfAttack[Random.Range(0, 2)].Play();
            playOnce2 = true;
        }else if(!hurtPlayerOnContact.attackingAnimation)
        {
            playOnce2 = false;
        }

        animator.SetBool("isRunning", wanderWalkController.isRunning);
        animator.SetBool("isWalking", wanderWalkController.isWalking);
        animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
        animator.SetBool("isAttacking", hurtPlayerOnContact.attackingAnimation);
        animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
    }
}
