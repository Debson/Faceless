using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO still bugged!!
public class WanderWalkController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 50f;

    [SerializeField]
    float followSpeed = 3f;

    [SerializeField]
    float timeBeforeFirstWander = 2;

    [SerializeField]
    float minTimeBetweenReconsideringDirection = 1;

    [SerializeField]
    float maxTimeBetweenReconsideringDirection = 6;

    [SerializeField]
    public float playerRange;

    [SerializeField]
    public LayerMask playerLayer;

    [SerializeField]
    float attackRange = 1f;

    [SerializeField]
    private float verticalAttackRange = 0f;

    [SerializeField]
    private float distanceToRunScaler;

    Animator animator;
    Rigidbody2D myBody;
    PlayerController playerController;
    Collider2D myCollider;
    Canvas healthBarCanvas;
    OrcController orcController;
    HurtEnemyOnContact enemy;
    FloorDetector floorDetector;

    public bool isRunning { set; get; }
    public bool playerInRange { set; get; }
    public bool isWalking { set; get; }
    public bool isAttacking { get; set; }
    public bool stunned { get; private set; }

    private float enemyYBounds;
    private float desiredWalkDirection;
    private float characterXBounds;
    private float distanceToPlayer;

    private bool isFlippedRigid;
    private bool usingRigid;
    private bool stopWalking;
    private bool callOnce;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        myCollider = GetComponent<Collider2D>();
        enemyYBounds = myCollider.bounds.size.y;
        characterXBounds = playerController.GetComponent<Collider2D>().bounds.size.x + attackRange;
        animator = GetComponentInChildren<Animator>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
        orcController = GetComponent<OrcController>();
        enemy = GetComponent<HurtEnemyOnContact>();
        floorDetector = FindObjectOfType<FloorDetector>();
    }

    protected void Start()
    {
            StartCoroutine(Wander());
    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        RotateEnemy();

        CheckIfCloseToPlayer();

        StartCoroutine(Move());
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
                }
                else
                {
                    float desiredXVelocity = desiredWalkDirection * walkSpeed * Time.deltaTime;
                    myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
                    usingRigid = true;
                    isRunning = false;
                }
            }

            //WALKING
            if (!playerInRange)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            /*
            if ((transform.position.y + enemyYBounds + verticalAttackRange) > playerController.transform.position.y)
            {
                isWalking = false;
            }
            else
            {
                isWalking = true;
            }
            */
        }
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
            else if (distanceToPlayer > characterXBounds * distanceToRunScaler )
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
                //healthBarCanvas.enabled = true;
                isFlippedRigid = true;
            }

            if (myBody.velocity.x < .1f && isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 180, 0);
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFlippedRigid = false;
            }
        }
        else
        {
            if ((transform.position.x > playerController.transform.position.x) && !isFlippedRigid)
            {
                healthBarCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
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

