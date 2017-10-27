using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatMovement : MonoBehaviour
{
    static readonly Quaternion flipRotation = Quaternion.Euler(0, 0, 1);

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float playerRange;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    float attackRange = 1.5f;

    PlayerController playerController;
    EnemyHealthManager enemyHealthManager;
    Rigidbody2D myBody;

    private bool playerInRange;

    private float timeToFall = 1f;
    private float fallSpeed = 7;
    private bool stopWalking;
    private float characterXBounds;

    protected void Awake()
    {
        characterXBounds = GetComponent<Collider2D>().bounds.size.x + attackRange;
        playerController = FindObjectOfType<PlayerController>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }

   
    protected void Update()
    {
        float XPosition = transform.position.x;
        if (enemyHealthManager.GetHealth() <= 0)
        {
            playerInRange = false;

            float desiredYVelocity = fallSpeed * Time.deltaTime;
            transform.position = new Vector2(XPosition, transform.position.y - desiredYVelocity);
            // Rotate bat while falling down
            transform.rotation *= flipRotation;
        }

        if (!stopWalking)
        {
            playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

            if (playerInRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position,
                                                         moveSpeed * Time.deltaTime);
            }
        }

        if (Vector3.Distance(playerController.transform.position, transform.position) < characterXBounds)
        {
            stopWalking = true;
        }
        else
        {
            stopWalking = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
