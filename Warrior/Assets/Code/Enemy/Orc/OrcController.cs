using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour
{

    WanderWalkController wanderWalkController;
    HurtEnemyOnContact hurtEnemyOnContact;
    FloorDetector floorDetector;
    EnemyHealthManager enemyHealthManager;
    Animator animator;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        floorDetector = FindObjectOfType<FloorDetector>();
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        if(enemyHealthManager.GetHealth() <= 0)
        {
            animator.SetBool("isDead", true);

            Destroy(gameObject, t: 100f);
        }
        else
        {
            animator.SetBool("isRunning", wanderWalkController.isRunning);
            animator.SetBool("isWalking", !wanderWalkController.playerInRange);
            animator.SetBool("isHurted", hurtEnemyOnContact.hitOnlyOnce);
            animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
        }

    }

}
