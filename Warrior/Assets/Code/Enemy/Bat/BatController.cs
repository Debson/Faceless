using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBar;

    [SerializeField]
    private float minDistToFly = 5f;

    [SerializeField]
    private LayerMask layerMask;

    WanderWalkController wanderWalkController;
    HurtPlayerOnContact hurtPlayerOnContact;
    Animator animator;
    PlayerController player;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;
    EnemyHealthManager enemyHealthManager;
    SpriteRenderer sprite;
    Collider2D myCollider;
    Rigidbody2D mybody;

    private Vector2 startingPos;
    private float distance;

    private bool isDead;
    private bool playOnce;
    private float playerYBounds;
    private float playerYPos;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        sprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        mybody = GetComponent<Rigidbody2D>();
        playerYBounds = player.GetComponent<Collider2D>().bounds.size.y * 0.3f;
        playerYPos = player.transform.position.y;
    }

    protected void Start()
    {
        wanderWalkController.StopWander = true;
        startingPos = transform.position;
        animator.SetBool("Idle", true);
    }

    protected void LateUpdate()
    {
        if(enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            OnDeath();
        }
        else if(!isDead)
        {
            SetAnimationLogic();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, minDistToFly, layerMask);
            Debug.DrawRay(transform.position, new Vector2(0, -minDistToFly), Color.red);
            if(hit.collider != null)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + Time.deltaTime * minDistToFly);
            }
        }
    }

    private void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        transform.rotation = Quaternion.Euler(0, 0, 70);
        wanderWalkController.enabled = false;
        hurtPlayerOnContact.enabled = false;
        hurtEnemyOnContact.enabled = false;
        myCollider.enabled = false;
        mybody.gravityScale = 10f;
        Destroy(gameObject, 6f);
    }

    private void SetAnimationLogic()
    {
        if (wanderWalkController.isWalking)
        {
            animator.SetFloat("flySpeed", 0.6f);
            StartCoroutine(BackToFly());
        }
        else
        {
            animator.SetFloat("flySpeed", 0.8f);
        }

        if (hurtPlayerOnContact.attackingAnimation)
        {
            // sounds
        }
        else
        {
            playOnce = false;
        }

        if (wanderWalkController.playerInRange)
        {
            animator.SetBool("Idle", false);
            healthBar.active = true;
            // sounds
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
