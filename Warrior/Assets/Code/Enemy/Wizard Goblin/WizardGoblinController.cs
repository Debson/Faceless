using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardGoblinController : MonoBehaviour
{
    [SerializeField] private Collider2D deathCollider;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;
    Rigidbody2D myBody;
    Collider2D myCollider;
    EnemyHealthBar enemyHealthBar;

    private Vector2 startingPos;

    private bool playOnce;
    private bool isDead;

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
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        deathCollider.enabled = false;
    }

    protected void Start()
    {
        startingPos = transform.position;
        animator.SetBool("Walk", true);
    }

    protected void LateUpdate()
    {
        if(enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            OnDeath();
        }
        else if(!isDead)
        {
            SetAnimationsLogic();
        }
    }

    private void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        myBody.isKinematic = false;
        myBody.gravityScale = 2f;
        myCollider.enabled = false;
        deathCollider.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, -40);
        wanderWalkController.enabled = false;
        hurtEnemyOnContact.enabled = false;
        enemyHealthBar.enabled = false;
        Destroy(gameObject, 6f);
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

    private void SetAnimationsLogic()
    {
        if (hurtPlayerOnContact.attackingAnimation)
        {
            Attack();
        }
        else
        {
            playOnce = false;
        }

        if (wanderWalkController.isWalking)
        {
            StartCoroutine(BackToFly());
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