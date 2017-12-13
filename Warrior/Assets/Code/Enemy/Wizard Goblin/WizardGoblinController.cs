using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardGoblinController : MonoBehaviour
{
    [SerializeField]
    private GameObject deathCollider;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;
    Rigidbody2D myBody;
    Collider2D myCollider;

    private Vector2 startingPos;

    private bool playOnce;
    private bool dead;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        startingPos = transform.position;
        animator.SetBool("Walk", true);
    }

    protected void LateUpdate()
    {
        SetAnimations();

        if (hurtPlayerOnContact.attackingAnimation)
        {
            Attack();
        }
        else
        {
            playOnce = false;
        }

        if(wanderWalkController.isWalking)
        {
            StartCoroutine(BackToFly());
        }
    }

    IEnumerator BackToFly()
    {
        while((transform.position.y + 0.1f <= startingPos.y) || transform.position.y - 0.1f >= startingPos.y)
        {
            transform.position = Vector2.LerpUnclamped(transform.position, new Vector2(transform.position.x, startingPos.y), Time.deltaTime * 0.005f);
            yield return 0;
        }
    }

    private void Attack()
    {
        if (!playOnce)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Attack");
            playOnce = true;
        }
    }

    private void SetAnimations()
    {
        if(enemyHealthManager.GetHealth() <= 0 && !dead)
        {
            animator.SetTrigger("Dead");
            myBody.isKinematic = false;
            myBody.gravityScale = 2f;
            myCollider.enabled = false;
            deathCollider.active = true;
            transform.rotation = Quaternion.Euler(0, 0, -40);
            wanderWalkController.enabled = false;
            hurtEnemyOnContact.enabled = false;
            Destroy(gameObject, 6f);
            dead = true;
        }

        if (hurtEnemyOnContact.isHurt)
        {
            animator.SetTrigger("Hurt");
        }

        if (wanderWalkController.isRunning)
        {
            animator.SetFloat("walkSpeed", 0.2f);
        }
        else if (!wanderWalkController.isRunning)
        {
            animator.SetFloat("walkSpeed", 0.08f);
        }

        if (wanderWalkController.isWalking || wanderWalkController.isRunning)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }
}