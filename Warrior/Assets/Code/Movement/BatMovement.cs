using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatMovement : MonoBehaviour
{
    static readonly Quaternion flipRotation = Quaternion.Euler(0, 0, 1);

    private PlayerController playerController;

    [SerializeField]
    public float moveSpeed;

    [SerializeField]
    public float playerRange;

    [SerializeField]
    public LayerMask playerLayer;

    EnemyHealthManager enemyHealthManager;
    Rigidbody2D myBody;

    public bool playerInRange;

    private float timeToFall = 1f;
    private float fallSpeed = 7;

    protected void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }

   
    protected void Update()
    {
        float XPosition = transform.position.x;
        
        if (enemyHealthManager.enemyHealth <= 0)
        {
            playerInRange = false;

            float desiredYVelocity = fallSpeed * Time.deltaTime;
            transform.position = new Vector2(XPosition, transform.position.y - desiredYVelocity);
            // Rotate bat while falling down
            transform.rotation *= flipRotation;
        }

        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
        if (playerInRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position,
                                                     moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
