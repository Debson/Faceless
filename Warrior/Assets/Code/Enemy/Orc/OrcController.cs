using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour
{
    [SerializeField]
    Collider2D deadCollider;

    WanderWalkController wanderWalkController;
    HurtEnemyOnContact hurtEnemyOnContact;
    HurtPlayerOnContact hurtPlayerOnContact;
    FloorDetector floorDetector;
    EnemyHealthManager enemyHealthManager;
    Animator animator;
    Canvas healthBarCanvas;
    Collider2D myCollider;



    private bool isDead;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        hurtPlayerOnContact = GetComponentInChildren<HurtPlayerOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        floorDetector = FindObjectOfType<FloorDetector>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    protected void Update()
    {


        if (!isDead)
        {
            if (enemyHealthManager.GetHealth() <= 0)
            {
                animator.SetTrigger("isDead");
                isDead = true;
                wanderWalkController.enabled = false;
                hurtPlayerOnContact.enabled = false;
                healthBarCanvas.enabled = false;
                myCollider.enabled = false;
                deadCollider.enabled = true;

                Destroy(gameObject, 7f);
            }
            else
            {
                animator.SetBool("isRunning", wanderWalkController.isRunning);
                animator.SetBool("isWalking", wanderWalkController.isWalking);
                animator.SetBool("isHurted", hurtEnemyOnContact.hitOnlyOnce);
                animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
                animator.SetBool("isAttacking", hurtPlayerOnContact.attackingAnimation);
                animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        // On exit from Troll's attack trigger collider reset variables responsible for attack delay on first contact with Troll
        if (collision.tag == "Player")
        {
            hurtPlayerOnContact.isHurted = false;
        }
    }*/
}
