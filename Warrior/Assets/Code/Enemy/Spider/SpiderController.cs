using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    HurtPlayerOnContact hurtPlayerOnContact;
    EnemyHealthManager enemyHealthManager;
    FloorDetector floorDetector;
    Animator animator;
    Collider2D[] colliderList;
    WanderWalkController wanderWalkController;
    AudioManager audioManager;

    private bool onlyOnce;


    protected void Awake()
    {
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        floorDetector = FindObjectOfType<FloorDetector>();
        animator = GetComponent<Animator>();
        colliderList = GetComponents<Collider2D>();
        wanderWalkController = GetComponent<WanderWalkController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    protected void Start()
    {
    }

    protected void Update()
    {
        if (enemyHealthManager.GetHealth() <= 0)
        {
            animator.SetTrigger("isDead");
            var wanderWalkController = GetComponent<WanderWalkController>().enabled = false;
            var canvas = GetComponentInChildren<Canvas>().enabled = false;
            //var rigidbody = GetComponent<Rigidbody2D>()
            foreach(Collider2D col in colliderList)
            {
                col.enabled = false;
            }
            audioManager.spiderChattering.Stop();
            Destroy(gameObject, 5f);
        }
        else
        {
            animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
            animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);

            if (hurtPlayerOnContact.attackingAnimation && onlyOnce)
            {
                animator.SetTrigger("isAttacking");
                audioManager.spiderAttack.Play();
                onlyOnce = false;
            }
            else if (!hurtPlayerOnContact.attackingAnimation)
            {
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
}
