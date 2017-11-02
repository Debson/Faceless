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

    public float throwScytheDirection;

    private bool isAttacking;
    private bool isWalking;

    private bool playerInRange;
    private bool playerInAttackingRange;
    private bool isCoroutineExecuting;
    private bool isFacingLeft;
    private bool isWalkingLeft;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        playerBody = playerController.GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        playerInAttackingRange = Physics2D.OverlapCircle(transform.position, playerAttackingRange, playerLayer);

        float desiredXVelocity = 1.5f;

        if (playerInAttackingRange && !playerInRange)
        {
            animator.SetBool("isWalking", false);

            if (transform.position.x > playerController.transform.position.x && !isFacingLeft)
            {
                    transform.rotation *= Quaternion.Euler(0, -180, 0);
                    throwScytheDirection = -1;
                    isFacingLeft = true;
                    isWalkingLeft = false;
            }
            else if (transform.position.x < playerController.transform.position.x && isFacingLeft)
            {
                    transform.rotation *= Quaternion.Euler(0, -180, 0);
                    throwScytheDirection = 1;
                    isFacingLeft = false;
                    isWalkingLeft = false;
            }

        }

        if (playerInRange)
        {
            animator.SetBool("isWalking", true);

            if (transform.position.x > playerController.transform.position.x && isFacingLeft)
            {
                transform.rotation *= Quaternion.Euler(0, -180, 0);
                isFacingLeft = false;
                isWalkingLeft = false;
            }

            if(transform.position.x < playerController.transform.position.x && !isFacingLeft)
            {
                transform.rotation *= Quaternion.Euler(0, -180, 0);
                isFacingLeft = true;
                isWalkingLeft = true;
            }

            if(!isWalkingLeft)
            {
                myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
            }

            if(isWalkingLeft)
            {
                myBody.velocity = new Vector2(-desiredXVelocity, myBody.velocity.y);
            }
        }

        if ((Mathf.Abs(playerBody.velocity.x) < 0.1f && playerInRange && !isAttacking))
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

    IEnumerator ThrowScythe()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.4f);
        Instantiate(scythe, throwPoint.position, throwPoint.rotation);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    IEnumerator ThrowScytheAfterTime()
    {
        if(isWalkingLeft)
        {
            throwScytheDirection = 1;
        }
        else
        {
            throwScytheDirection = -1;
        }

        yield return new WaitForSeconds(1f);
        transform.rotation *= Quaternion.Euler(0, -180, 0);
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.4f);
        Instantiate(scythe, throwPoint.position, throwPoint.rotation);
        transform.rotation *= Quaternion.Euler(0, -180, 0);
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
        Gizmos.DrawWireSphere(transform.position, playerAttackingRange);
    }

}
