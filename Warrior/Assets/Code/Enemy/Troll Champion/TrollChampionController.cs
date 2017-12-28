using System.Collections;
using UnityEngine;

public class TrollChampionController : MonoBehaviour
{

    [SerializeField]
    Collider2D deathCollider;

    WanderWalkController wanderWalkController;
    HurtEnemyOnContact hurtEnemyOnContact;
    HurtPlayerOnContact hurtPlayerOnContact;
    FloorDetector floorDetector;
    EnemyHealthManager enemyHealthManager;
    Animator animator;
    Collider2D myCollider;
    PlayerController playerController;
    Rigidbody2D myBody;
    AudioManager audioManager;
    EnemyHealthBar enemyHealthBar;

    private bool isDead;
    private bool jump;
    private bool firstJump;
    private bool callOnceRunning;
    private bool callOnceAttacking;
    private bool callOnceHurt;

    private float trollYBounds;

    protected void Awake()
    {
        wanderWalkController = GetComponent<WanderWalkController>();
        hurtEnemyOnContact = GetComponent<HurtEnemyOnContact>();
        hurtPlayerOnContact = GetComponentInChildren<HurtPlayerOnContact>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
        floorDetector = FindObjectOfType<FloorDetector>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        playerController = FindObjectOfType<PlayerController>();
        myBody = GetComponent<Rigidbody2D>();
        trollYBounds = GetComponent<Collider2D>().bounds.size.y;
        audioManager = FindObjectOfType<AudioManager>();
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        deathCollider.enabled = false;
    }

    protected void Start()
    {
        callOnceRunning = true;
        callOnceAttacking = true;
        callOnceHurt = true;
        jump = true;
    }

    protected void LateUpdate()
    {
        if (enemyHealthManager.GetHealth() <= 0 && !isDead)
        {
            OnDeath();
            SetSounds();
        }
        else if (!isDead)
        {
            SetAnimationLogic();
            SetSounds();
        }
    }

    private void SetSounds()
    {
        if (wanderWalkController.playerInRange && callOnceRunning)
        {
            audioManager.OrcRoar[2].Play();
            callOnceRunning = false;
        }
        else if (!wanderWalkController.playerInRange)
        {
            callOnceRunning = true;
        }

        if (hurtPlayerOnContact.attackingAnimation && callOnceAttacking)
        {
            audioManager.orcWeapon[0].Play();
            callOnceAttacking = false;
        }
        else if (!hurtPlayerOnContact.attackingAnimation)
        {
            callOnceAttacking = true;
        }

        if (hurtEnemyOnContact.isHurt && callOnceHurt)
        {
            audioManager.orcPain2[Random.Range(1, 3)].Play();
            callOnceHurt = false;
        }
        else if (!hurtEnemyOnContact.isHurt)
        {
            callOnceHurt = true;
        }
    }

    private void OnDeath()
    {
        audioManager.orcDeath[2].Play();

        gameObject.GetComponent<Collider2D>().enabled = false;
        deathCollider.enabled = true;
        wanderWalkController.enabled = false;
        hurtPlayerOnContact.enabled = false;
        hurtEnemyOnContact.enabled = false;
        enemyHealthBar.enabled = false;
        myCollider.enabled = false;
        animator.SetTrigger("isDead");
        isDead = true;
        Destroy(gameObject, 7f);
    }

    private void SetAnimationLogic()
    {
        animator.SetBool("isRunning", wanderWalkController.isRunning);
        animator.SetBool("isWalking", wanderWalkController.isWalking);
        animator.SetBool("isHurted", hurtEnemyOnContact.hitOnlyOnce);
        animator.SetBool("isTouchingFloor", floorDetector.isTouchingFloor);
        animator.SetBool("isAttacking", hurtPlayerOnContact.attackingAnimation);
        animator.SetBool("isInTrigger", hurtPlayerOnContact.isInTrigger);
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
