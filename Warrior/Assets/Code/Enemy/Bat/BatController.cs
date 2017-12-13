using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBar;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController playerController;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;
    SpriteRenderer sprite;
    Collider2D myCollider;
    Rigidbody2D mybody;

    private Vector2 startingPos;
    private float distance;

    private bool _death;
    private bool playOnce;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        sprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        mybody = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        wanderWalkController.StopWander = true;
        startingPos = transform.position;
        animator.SetBool("Idle", true);
    }

    protected void LateUpdate()
    {
        if (hurtPlayerOnContact.attackingAnimation)
        {
            // sounds
        }
        else
        {
            playOnce = false;
        }

        if(wanderWalkController.playerInRange)
        {
            animator.SetBool("Idle", false);
            healthBar.active = true;
            // sounds
        }

        if(enemyHealthManager.GetHealth() <= 0 && !_death)
        {
            animator.SetTrigger("Dead");
            transform.rotation = Quaternion.Euler(0, 0, 70);
            wanderWalkController.enabled = false;
            hurtPlayerOnContact.enabled = false;
            myCollider.enabled = false;
            mybody.gravityScale = 1f;
            Destroy(gameObject, 6f);
        }

        if (wanderWalkController.isWalking)
        {
            animator.SetFloat("flySpeed", 0.6f);
            StartCoroutine(BackToFly());
        }
        else
        {
            animator.SetFloat("flySpeed", 0.8f);
        }
    }

    IEnumerator BackToFly()
    {
        if (startingPos.x > transform.position.x)
        {
            sprite.flipX = false;
        }

        while ((transform.position.y + 0.1f <= startingPos.y) || transform.position.y - 0.1f >= startingPos.y)
        {
            transform.position = Vector2.LerpUnclamped(transform.position, startingPos, Time.deltaTime * 0.005f);
            if (wanderWalkController.playerInRange)
            {
                sprite.flipX = true;
                StopAllCoroutines();
            }
            yield return 0;
        }
        sprite.flipX = true;
        animator.SetBool("Idle", true);
        healthBar.active = false;
    }
}
