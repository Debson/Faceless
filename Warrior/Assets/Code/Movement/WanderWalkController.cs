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

    private float desiredWalkDirection;
    private bool isFlippedRigid;
    private bool playerInRange;
    private bool usingRigid;

    Rigidbody2D myBody;
    PlayerController playerController;

    protected void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
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
                myBody.transform.rotation = Quaternion.Euler(0, -180, 0);
                isFlippedRigid = true;
            }

            if (myBody.velocity.x < .1f && isFlippedRigid)
            {
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
                isFlippedRigid = false;
            }
        }
        else
        {
            if((transform.position.x > playerController.transform.position.x) && !isFlippedRigid)
            {
                myBody.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if ((transform.position.x < playerController.transform.position.x) && !isFlippedRigid)
            {
                myBody.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                myBody.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }
        
        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (playerInRange && (transform.position.y + 2 >= playerController.transform.position.y))
        {
            transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position,
                                                     followSpeed * Time.deltaTime);
            usingRigid = false;
        }
        else
        {
            float desiredXVelocity = desiredWalkDirection * walkSpeed * Time.deltaTime;
            myBody.velocity = new Vector2(desiredXVelocity, myBody.velocity.y);
            usingRigid = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}

