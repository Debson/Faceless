using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderWalkController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 50f;

    [SerializeField]
    float followSpeed = 3f;

    [SerializeField]
    private float timeToWaitAfterAttack = 0.5f;

    [SerializeField]
    float timeBeforeFirstWander = 2;

    [SerializeField]
    float minTimeBetweenReconsideringDirection = 1;

    [SerializeField]
    float maxTimeBetweenReconsideringDirection = 6;

    [SerializeField]
    private bool enableWander = true;

    [SerializeField]
    public float playerRange;

    [SerializeField]
    public LayerMask playerLayer;

    [SerializeField]
    float attackRange = 1f;

    [SerializeField]
    private float verticalAttackRange = 0f;

    [SerializeField]
    private float distanceToRunScaler = 1f;

    Animator animator;
    Rigidbody2D myBody;
    PlayerController playerController;
    Collider2D myCollider;
    Canvas healthBarCanvas;
    HurtEnemyOnContact enemy;
    FloorDetector floorDetector;
    HurtPlayerOnContact hurtPlayerOnContact;

    public bool IgnorePlayerAboveEnemy { get; set; }
    public bool isRunning { set; get; }
    public bool playerInRange { set; get; }
    public bool isWalking { set; get; }
    public bool isAttacking { get; set; }
    public bool stunned { get; private set; }
    public bool isIdle { get; private set; }
    public bool isFacingLeft { get; private set; }
    public bool StopWander { get; set; }

    private float enemyYBounds;
    private float desiredWalkDirection;
    private float characterXBounds;
    private float distanceToPlayer;

    private bool isFlippedRigid;
    private bool usingRigid;
    private bool stopWalking;
    private bool callOnce;
    private bool stopRotate;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        myCollider = GetComponent<Collider2D>();
        enemyYBounds = myCollider.bounds.size.y;
        characterXBounds = playerController.GetComponent<Collider2D>().bounds.size.x + attackRange;
        animator = GetComponentInChildren<Animator>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
        enemy = GetComponent<HurtEnemyOnContact>();
        floorDetector = FindObjectOfType<FloorDetector>();
        hurtPlayerOnContact = GetComponent<HurtPlayerOnContact>();
    }

    protected void Start()
    {
        if (!StopWander && enableWander)
        {
            StartCoroutine(Wander());
        }
    }

    protected void LateUpdate()
    {
        if (playerController == null)
        {
            isIdle = true;
            return;
        }
        else
        {
            playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

            StartCoroutine(Move());
        }
    }

    IEnumerator Wander()
    {
        desiredWalkDirection = 1;

        if (timeBeforeFirstWander > 0)
        {
            float timeToSleep = timeBeforeFirstWander + GetRandomTimeToSleep();
            yield return new WaitForSeconds(timeToSleep);
        }

        while (true)
        {
            SelectARandomWalkDirection();
            float timeToSleep = GetRandomTimeToSleep();
            yield return new WaitForSeconds(timeToSleep);
        }
    }

    IEnumerator Move()
    {
        if (hurtPlayerOnContact.attackingAnimation && !stopRotate)
        {
            stopRotate = true;
            isRunning = false;
            yield return new WaitForSeconds(timeToWaitAfterAttack);
            stopRotate = false;
        }

        if (!stopRotate)
        {
            RotateEnemy();
            CheckIfCloseToPlayer();

            if (!stopWalking)
            {
                if (enemy.comboEnabled && enemy.isHurt && !callOnce)
                {
                    stunned = true;
                    callOnce = true;
                }

                if (stunned && callOnce)
                {
                    callOnce = false;
                    yield return new WaitForSeconds(enemy.stunTime);
                    stunned = false;
                }
                else if (!stunned && !callOnce)
                {
                    if (playerInRange && (transform.position.y + enemyYBounds + verticalAttackRange >= playerController.transform.position.y))
                    {
                        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position,
                                                                 followSpeed * Time.deltaTime);
                        usingRigid = false;
                        isRunning = true;
                        isWalking = false;
                    }
                    else if (playerInRange && !(transform.position.y + enemyYBounds + verticalAttackRange >= playerController.transform.position.y) || !playerInRange)
                    {
                        float desiredXVelocity = desiredWalkDirection * walkSpeed * Time.deltaTime;
                        myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
                        usingRigid = true;
                        isRunning = false;
                        isWalking = true;
                    }
                }
            }
        }
    }

    IEnumerator EnemyStunned(float time)
    {
        isRunning = false;
        stopRotate = true;
        yield return new WaitForSeconds(time);
        stopRotate = false;
    }
    
    public void StunEnemy(float time)
    {
        StartCoroutine(EnemyStunned(time));
    }

    private float GetRandomTimeToSleep()
    {
        return Random.Range(minTimeBetweenReconsideringDirection, maxTimeBetweenReconsideringDirection);
    }

    private void SelectARandomWalkDirection()
    {
        desiredWalkDirection = Random.value <= .5f ? 1 : -1;
    }

    private void CheckIfCloseToPlayer()
    {
        distanceToPlayer = Vector3.Distance(playerController.transform.position, transform.position);
        if (floorDetector.DetectTheFloorWeAreStandingOn() == null)
        {
            return;
        }
        else
        {
            if (distanceToPlayer < characterXBounds || floorDetector.DetectTheFloorWeAreStandingOn().name == gameObject.name)
            {
                stopWalking = true;
                isRunning = false;
            }
            else if (distanceToPlayer > characterXBounds * distanceToRunScaler)
            {
                stopWalking = false;
            }
        }
    }

    private void RotateEnemy()
    {
        //If player is not in range, rotate enemy's body basing on velocity. If player is in range, rotate enemy's body basing on his position relative to enemy's body.
        if (usingRigid)
        {
            if (myBody.velocity.x > .1f && !isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 180, 0);
                myBody.transform.rotation = Quaternion.Euler(0, -180, 0);
                isFlippedRigid = true;
                isFacingLeft = false;
            }

            if (myBody.velocity.x < .1f && isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 180, 0);
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFlippedRigid = false;
                isFacingLeft = true;
            }
        }
        else
        {
            if ((transform.position.x > playerController.transform.position.x) && !isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFacingLeft = true;
            }
            else
            {
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFlippedRigid = false;
            }

            if ((transform.position.x < playerController.transform.position.x) && !isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 180, 0);
                myBody.transform.rotation = Quaternion.Euler(0, -180, 0);
                isFlippedRigid = true;
                isFacingLeft = false;
            }
            else
            {
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}

