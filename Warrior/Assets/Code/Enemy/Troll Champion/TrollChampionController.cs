using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollChampionController : MonoBehaviour
{

    [SerializeField]
    Collider2D deadCollider;

    [SerializeField]
    Collider2D attackTrigger;

    WanderWalkController wanderWalkController;
    HurtEnemyOnContact hurtEnemyOnContact;
    HurtPlayerOnContact hurtPlayerOnContact;
    FloorDetector floorDetector;
    EnemyHealthManager enemyHealthManager;
    Animator animator;
    Canvas healthBarCanvas;
    Collider2D myCollider;
    PlayerController playerController;
    Rigidbody2D myBody;

    private float trollYBounds;
    private float waitTime = 0f;
    private bool isDead;
    private bool jump = true;
    private bool firstJump;

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
        playerController = FindObjectOfType<PlayerController>();
        myBody = GetComponent<Rigidbody2D>();
        trollYBounds = GetComponent<Collider2D>().bounds.size.y;
    }

    protected void Update()
    {
        if (!isDead)
        {
            if (enemyHealthManager.GetHealth() <= 0)
            {
                animator.SetTrigger("isDead");
                isDead = true;

                attackTrigger.gameObject.SetActive(false);
                hurtEnemyOnContact.enabled = false;
                wanderWalkController.enabled = false;
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

   
    IEnumerator Jump()
    {


        yield return new WaitForSeconds(2f);

            if (!jump)
            {
            myBody.AddForce(new Vector2(0, myBody.mass * 11), ForceMode2D.Impulse);
            jump = true;
            firstJump = true;
            yield return new WaitForEndOfFrame();
        }

    }
}
