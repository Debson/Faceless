using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHunter : MonoBehaviour
{
    [SerializeField]
    private float playerRange;

    [SerializeField]
    private float playerAttackingRange;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    public Transform throwPoint;

    [SerializeField]
    public GameObject scythe;

    PlayerController playerController;
    Animator animator;
    Rigidbody2D myBody;
    Rigidbody2D playerBody;
    Transform healthBar;
    EnemyHealthManager enemyHealthManager;
    Collider2D collider;
    AudioManager audioManager;
    HurtEnemyOnContact hurtEnemyOnContact;

    public float throwScytheDirection { get; set; }

    private float desiredXVelocity;

    private bool isAttacking;
    private bool isWalking;
    private bool playerInRange;
    private bool playerInAttackingRange;
    private bool isCoroutineExecuting;
    private bool isFacingLeft;
    private bool isWalkingLeft;
    private bool playerEnteredPlayerRange;
    private bool onlyOnce;
    private bool isDead;
    private bool isInPlayerRange;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        playerBody = playerController.GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<Canvas>().transform;
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        collider = GetComponent<Collider2D>();
        audioManager = FindObjectOfType<AudioManager>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
    }

    protected void Start()
    {
        healthBar.localRotation *= Quaternion.Euler(0, -180, 0);
        onlyOnce = true;
    }

    protected void Update()
    {
        if(hurtEnemyOnContact.isHurt && onlyOnce)
        {
            audioManager.reaperHurt[Random.Range(0, 1)].Play();
        }
        else if(!hurtEnemyOnContact.isHurt)
        {
            onlyOnce = true;
        }

        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            audioManager.reaperDead[0].Play();
            animator.SetTrigger("isDead");
            collider.enabled = false;
            StopAllCoroutines();
            isDead = true;
            Destroy(gameObject, 7f);
        }
        else if(!isDead)
        {
            playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
            playerInAttackingRange = Physics2D.OverlapCircle(transform.position, playerAttackingRange, playerLayer);
            InRange();
        }
    }

    IEnumerator ThrowScythe()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.4f);
        audioManager.reaperAttack[Random.Range(0, 3)].Play();
        Instantiate(scythe, throwPoint.position, throwPoint.rotation);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    IEnumerator ThrowScytheAfterTime()
    {
        if (isWalkingLeft)
        {
            throwScytheDirection = 1;
        }
        else
        {
            throwScytheDirection = -1;
        }

        yield return new WaitForSeconds(1f);
        healthBar.localRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation *= Quaternion.Euler(0, -180, 0);
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.4f);
        audioManager.reaperAttack[Random.Range(0, 3)].Play();
        Instantiate(scythe, throwPoint.position, throwPoint.rotation);
        healthBar.localRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation *= Quaternion.Euler(0, -180, 0);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    private void InRange()
    {
        desiredXVelocity = 1.5f;
        if (playerInAttackingRange && !playerInRange)
        {
            isInPlayerRange = false;
            animator.SetBool("isWalking", false);
            if (transform.position.x > playerController.transform.position.x && !isFacingLeft)
            {
                if (playerEnteredPlayerRange)
                {
                    healthBar.localRotation *= Quaternion.Euler(0, 180, 0);
                    playerEnteredPlayerRange = false;
                }

                transform.rotation *= Quaternion.Euler(0, -180, 0);
                throwScytheDirection = -1;
                isFacingLeft = true;
                isWalkingLeft = false;
            }
            else if (transform.position.x < playerController.transform.position.x && isFacingLeft)
            {
                healthBar.localRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation *= Quaternion.Euler(0, -180, 0);
                throwScytheDirection = 1;
                isFacingLeft = false;
                isWalkingLeft = false;
                playerEnteredPlayerRange = true;
            }
        }

        if (playerInRange)
        {
            if(!isInPlayerRange)
            {
                audioManager.reaperAgro[1].Play();
                isInPlayerRange = true;
            }
            animator.SetBool("isWalking", true);
            if (transform.position.x > playerController.transform.position.x && (isFacingLeft || !playerEnteredPlayerRange))
            {
                if (!playerEnteredPlayerRange)
                {
                    healthBar.localRotation *= Quaternion.Euler(0, 180, 0);
                    playerEnteredPlayerRange = true;
                }
                else if (isFacingLeft)
                {
                    transform.rotation *= Quaternion.Euler(0, -180, 0);
                    isFacingLeft = false;
                    isWalkingLeft = false;
                }
            }

            if (transform.position.x < playerController.transform.position.x && !isFacingLeft)
            {
                healthBar.localRotation = Quaternion.Euler(0, 180, 0);
                transform.rotation *= Quaternion.Euler(0, -180, 0);
                isFacingLeft = true;
                isWalkingLeft = true;
                playerEnteredPlayerRange = false;
            }

            if (!isWalkingLeft)
            {
                myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
            }

            if (isWalkingLeft)
            {
                myBody.velocity = new Vector2(-desiredXVelocity, myBody.velocity.y);
            }
        }

        if (playerController.transform.position.y <= transform.position.y)
        {
            if ((playerInRange && !isAttacking))
            {
                StartCoroutine(ThrowScytheAfterTime());
                isAttacking = true;
            }
            else if ((playerInAttackingRange && !playerInRange && !isAttacking))
            {
                StartCoroutine(ThrowScythe());
                isAttacking = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
        Gizmos.DrawWireSphere(transform.position, playerAttackingRange);
    }
}
