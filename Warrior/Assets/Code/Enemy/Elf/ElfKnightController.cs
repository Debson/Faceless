using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfKnightController : MonoBehaviour
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

    private float direction;
    private bool isFacingRight;

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

    protected void Start()
    {
        direction = -1;
    }

    protected void Update()
    {
        if (hurtEnemyOnContact.isHurt)
        {
            animator.SetTrigger("isHurt");
            //audioManager.elfHurt[Random.Range(0, 2)].Play();
        }
        else
        {

        }
        SetAnimationLogic();
    }

    IEnumerator OnDeath()
    {
        if (enemyHealthManager.GetHealth() <= 0)
        {
            animator.SetTrigger("isDead");
            //audioManager.elfDeath[0].Play();
            //disableOnDeath.SetActive(false);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            if (transform.position.x < playerController.transform.position.x && !isFacingRight)
            {
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                enemyHealthBar.transform.rotation *= Quaternion.Euler(0, 180, 0);

                isFacingRight = true;
                direction = 1;
                yield return new WaitForSeconds(0.2f);
            }
            else if (transform.position.x > playerController.transform.position.x && isFacingRight)
            {
                transform.rotation *= Quaternion.Euler(0, -180, 0);
                enemyHealthBar.transform.rotation *= Quaternion.Euler(0, 180, 0);

                isFacingRight = false;
                direction = -1;
                yield return new WaitForSeconds(0.2f);
            }
        }
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
