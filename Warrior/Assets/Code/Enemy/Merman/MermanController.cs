using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermanController : MonoBehaviour
{
    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;

    private float walkSpeed = 1.5f;

    private bool playOnce;
    private bool _Grounded;
    private bool _Attack;
    private bool _Dead;
    private bool _Walking;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }

    protected void Start()
    {
        animator.SetBool("isWalking", true);
    }

    protected void Update()
    {
        if(hurtPlayerOnContact.attackingAnimation)
        {
            StartCoroutine(TongueAttack());
        }
        else
        {
            playOnce = false;
        }

        if(wanderWalkController.isRunning)
        {
            animator.SetFloat("walkingSpeed", 0.6f);
        }
        else if(!wanderWalkController.isRunning)
        {
            animator.SetFloat("walkingSpeed", 0.2f);
        }

        if(wanderWalkController.isWalking || wanderWalkController.isRunning)
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
