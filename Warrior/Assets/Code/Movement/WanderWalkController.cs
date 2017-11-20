using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO still bugged!!
public class WanderWalkController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 50f;

    [SerializeField]
    float followSpeed = 0.7f;

    [SerializeField]
    float timeBeforeFirstWander = 10;

    [SerializeField]
    float minTimeBetweenReconsideringDirection = 1;

    [SerializeField]
    float maxTimeBetweenReconsideringDirection = 10;

    [SerializeField]
    public float playerRange;

    [SerializeField]
    public LayerMask playerLayer;

    [SerializeField]
    float attackRange = 1f;

    [SerializeField]
    float verticalRange = 0;

    [HideInInspector]
    public bool isRunning;

    [HideInInspector]
    public bool playerInRange;

    [HideInInspector]
    public bool isWalking;

    private float enemyYBounds;
    private float desiredWalkDirection;
    private float characterXBounds;
    private float distanceToPlayer;

    private bool isFlippedRigid;
    private bool usingRigid;
    private bool stopWalking;


    

    public bool isAttacking
    {
        get; set;
    }

    Animator animator;
    Rigidbody2D myBody;
    PlayerController playerController;
    Collider2D myCollider;
    Canvas healthBarCanvas;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        myCollider = GetComponent<Collider2D>();
        enemyYBounds = myCollider.bounds.size.y;
        characterXBounds = playerController.GetComponent<Collider2D>().bounds.size.x + attackRange;
        animator = GetComponentInChildren<Animator>();
        healthBarCanvas = GetComponentInChildren<Canvas>();
    }


    protected void Start()
    {
            StartCoroutine(Wander());
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

    void SelectARandomWalkDirection()
    {
        desiredWalkDirection = UnityEngine.Random.value <= .5f ? 1 : -1;
    }

    float GetRandomTimeToSleep()
    {
        return UnityEngine.Random.Range(
          minTimeBetweenReconsideringDirection,
          maxTimeBetweenReconsideringDirection);
    }

    protected void FixedUpdate()
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


        distanceToPlayer = Vector3.Distance(playerController.transform.position, transform.position);
        if (distanceToPlayer < characterXBounds)
        {
            stopWalking = true;
            isRunning = false;
        }
        else
        {
            stopWalking = false;
        }

        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (!stopWalking)
        {
            if (playerInRange && (transform.position.y + enemyYBounds + verticalRange >= playerController.transform.position.y))
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


            //WALKING
            if(!playerInRange)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if(transform.position.y + enemyYBounds + verticalRange >= playerController.transform.position.y)
            {
                isWalking = false;
            }
            else
            {
                isWalking = true;
            }
        }
    }


    IEnumerator StopWalking()
    {
        yield return 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}

