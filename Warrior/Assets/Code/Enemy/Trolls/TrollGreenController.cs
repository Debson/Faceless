using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WanderWalkController))]
[RequireComponent(typeof(EnemyHealthManager))]
[RequireComponent(typeof(EnemyHealthBar))]
[RequireComponent(typeof(HurtEnemyOnContact))]
[RequireComponent(typeof(HurtPlayerOnContact))]
[RequireComponent(typeof(Rigidbody2D))]
public class TrollGreenController : MonoBehaviour
{
    EnemyHealthBar enemyHealthBar;
    PlayerController playerController;
    Animator animator;
    EnemyHealthManager enemyHealthManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    AudioManager audioManager;
    FloorDetector floorDetector;
    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;

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
    }

    protected void Update()
    {
        SetAnimationLogic();
    }

    private void SetAnimationLogic()
    {
        animator.SetBool("isRunning", wanderWalkController.isRunning);
        animator.SetBool("isWalking", wanderWalkController.isWalking);
        animator.SetBool("isHurted", hurtEnemyOnContact.hitOnlyOnce);
        animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
        animator.SetBool("isAttacking", hurtPlayerOnContact.attackingAnimation);
        animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
    }
}

